using System.Security.Claims;
using Delta.Polling.Services.UserRole;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Delta.Polling.WebAPI.Infrastructure.Authentication;

public class CustomJwtBearerEvents(IUserRoleService userRoleService) : JwtBearerEvents
{
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var principal = context.Principal!;
        var identity = (principal.Identity as ClaimsIdentity)!;
        var username = principal.FindFirstValue(JwtClaimTypes.PreferredUserName);

        if (!string.IsNullOrWhiteSpace(username))
        {
            await ProcessUserRoles(identity, username);
        }
    }

    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        if (context.Exception is SecurityTokenExpiredException)
        {
            context.Response.Headers.Append("IsTokenExpired", true.ToString());
        }

        await Task.CompletedTask;
    }

    private async Task ProcessUserRoles(ClaimsIdentity identity, string username)
    {
        var roleNames = await userRoleService.GetUserRolesAsync(username);

        foreach (var roleName in roleNames)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
        }
    }
}
