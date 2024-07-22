using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Auth.Data;
using Service_Auth.Models.Identity;

using static Duende.IdentityServer.IdentityServerConstants;

namespace Service_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(LocalApi.PolicyName)]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;


        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var role = new ApplicationRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' created successfully.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRoleToUser(string idUser, string idRole)
        {
            var user = await _userManager.FindByIdAsync(idUser);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var role = await _roleManager.FindByIdAsync(idRole);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            // Normalize the role name
            var normalizedRoleName = role.Name.ToUpperInvariant();

            

            var result = await _userManager.AddToRoleAsync(user, normalizedRoleName);

            if (result.Succeeded)
            {
                return Ok($"Role '{role.Name}' assigned to user '{user.UserName}'.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("RemoveRole")]
        public async Task<IActionResult> RemoveRoleFromUser(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return Ok($"Role '{roleName}' removed from user '{userEmail}'.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("ListRoles")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpGet("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("AssignDroitsToRole")]
        public async Task<IActionResult> AssignDroitsToRole([FromBody] AssignDroitsRequest request)
        {
            // Vérifiez si les données de la requête sont valides
            if (request == null || !request.DroitIds.Any())
            {
                return BadRequest("Invalid request: roleId and/or droitIds are missing.");
            }

            // Vérifiez si le rôle existe
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            if (role == null)
            {
                return NotFound($"Role with ID {request.RoleId} not found.");
            }

            var droitsToAdd = new List<Droit>();

            // Vérifiez si chaque droit existe
            foreach (var droitId in request.DroitIds)
            {
                var droit = await _dbContext.DROITS.FindAsync(droitId);
                if (droit != null)
                {
                    droitsToAdd.Add(droit);
                }
            }

            // Assignez les droits au rôle
            foreach (var droit in droitsToAdd)
            {
                // Vérifiez si le droit est déjà attribué pour éviter les duplications
                var roleDroit = await _dbContext.DROIT_ROLE
                    .FirstOrDefaultAsync(rd => rd.RoleId == request.RoleId && rd.DRT_ID == droit.DRT_ID);
                if (roleDroit == null)
                {
                    // Ajoutez le droit au rôle
                    roleDroit = new DROIT_ROLE { RoleId = request.RoleId, DRT_ID = droit.DRT_ID };
                    _dbContext.DROIT_ROLE.Add(roleDroit);
                }
            }

            // Enregistrez les modifications dans la base de données
            await _dbContext.SaveChangesAsync();

            // Retournez une réponse de succès
            return Ok(new { message = $"Droits assigned to role '{role.Name}' successfully." });
        }


        public class AssignDroitsRequest
        {
            public int RoleId { get; set; }
            public List<int> DroitIds { get; set; }
        }


        [HttpGet("GetDroitsForRole")]
        public async Task<IActionResult> GetDroitsForRole(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                return NotFound($"Role with ID {roleId} not found.");
            }

            var droits = await _dbContext.DROIT_ROLE
                                         .Where(dr => dr.RoleId == roleId)
                                         .Include(dr => dr.Droit)
                                         .Select(dr => dr.Droit)
                                         .ToListAsync();

            return Ok(droits);
        }


    }
}
