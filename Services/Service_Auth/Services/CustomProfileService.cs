using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service_Auth.Models.Identity;
using Service_Auth.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<CustomProfileService> _logger;
    private readonly ApplicationDbContext _dbContext;

    public CustomProfileService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<CustomProfileService> logger, ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var userId = context.Subject.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Subject)?.Value;

        if (userId == null)
        {
            _logger.LogInformation("User ID is null");
            return;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogInformation("User not found");
            return;
        }

        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Name, user.UserName),
            new Claim(JwtClaimTypes.Email, user.Email),
            new Claim("entite_id", user.entite_id?.ToString() ?? string.Empty)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

        foreach (var role in roles)
        {
            var roleEntity = await _roleManager.FindByNameAsync(role);
            if (roleEntity != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                claims.AddRange(roleClaims);

                var roleDroits = await _dbContext.DROIT_ROLE
                    .Where(rd => rd.RoleId == roleEntity.Id)
                    .Include(rd => rd.Droit)
                    .ToListAsync();

                foreach (var roleDroit in roleDroits)
                {
                    claims.Add(new Claim("droit", roleDroit.Droit.DRT_LIB));
                }
            }
        }

        context.IssuedClaims.AddRange(claims);
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }
}
