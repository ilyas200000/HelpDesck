using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service_Auth.Data;
using Service_Auth.Models;
using Service_Auth.Models.Identity;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Service_Auth.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return Ok(user);
        }
        [Authorize(LocalApi.PolicyName)]
        [HttpPost("register")]
        [Authorize(Policy = "RequireDroitParametrageCréationUtilisateurs")]

        public async Task<IActionResult> Register([FromBody] RegistrationViewModel model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is already in use
            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null)
            {
                return Conflict(new { message = "l'adresse email  est déjà utilisé" });
            }

            // Check if the username is already in use
            var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
            if (existingUserByUsername != null)
            {
                return Conflict(new { message = "Username est déjà utilisé." });
            }

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Matricule = model.Matricule
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }

            return BadRequest(result.Errors);
        }


        [Authorize(LocalApi.PolicyName)]
        [HttpGet("all")]
        [Authorize(Policy = "RequireDroitParametrageListeEtModificationUtilisateurs")]

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .ToListAsync();

            var userList = new List<UserDetails>();

            foreach (var user in users)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                var roles = user.UserRoles.Select(u => u.Role.Name).ToList();

                userList.Add(new UserDetails
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles,
                    entite_id = user.entite_id,
                    Matricule = user.Matricule,
                    Claims = userClaims.Select(c => new ClaimDto
                    {
                        Type = c.Type,
                        Value = c.Value
                    }).ToList()
                });
            }

            return Ok(userList);
        }
        [Authorize(LocalApi.PolicyName)]
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "RequireDroitParametrageListeEtModificationUtilisateurs")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("User deleted successfully");
            }

            return BadRequest("Failed to delete user");
        }

        [Authorize(LocalApi.PolicyName)]
        [HttpPost("assignEntity")]
        [Authorize(Policy = "RequireDroitParametrageListeEtModificationUtilisateurs")]
        public async Task<IActionResult> AssignEntityToUser(string userId, int enttSgId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var entity = await _context.ENTITE_SIEGE.FindAsync(enttSgId);
            if (entity == null)
            {
                return NotFound("Entity not found");
            }

            user.entite_id = enttSgId; // Assign the entity to the user

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("Entity assigned to user successfully");
            }

            return BadRequest("Failed to assign entity to user");
        }
    }
}
