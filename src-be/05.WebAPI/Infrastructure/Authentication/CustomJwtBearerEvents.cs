using System.Security.Claims;
using Delta.Polling.Services.UserRole;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Delta.Polling.WebAPI.Infrastructure.Authentication;

public class CustomJwtBearerEvents(
    IUserRoleService userRoleService,
    ILogger<CustomJwtBearerEvents> logger)
    : JwtBearerEvents
{
    public override Task MessageReceived(MessageReceivedContext context)
    {
        logger.LogInformation("Entering the CustomJwtBearerEvents.MessageReceived event ...");

        var jwt = context.Request.Headers.FirstOrDefault(x => x.Key == "Authorization");

        logger.LogInformation("JWT: {Token}", jwt.Value.ToString());

        return Task.CompletedTask;
    }

    public override Task Challenge(JwtBearerChallengeContext context)
    {
        logger.LogInformation("The JWT is being challenged.");

        return Task.CompletedTask;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var jwt = context.SecurityToken as JsonWebToken;

        if (jwt is not null)
        {
            logger.LogInformation("Entering the CustomJwtBearerEvents.TokenValidated event ...");
            logger.LogInformation("CustomJwtBearerEvents.TokenValidated JWT: {SecurityToken}", jwt);

            var principal = context.Principal!;
            var identity = (principal.Identity as ClaimsIdentity)!;
            var username = principal.FindFirstValue(KnownClaimTypes.PreferredUsername);

            logger.LogInformation("The value of Claim {ClaimType} in the JWT: {ClaimValue}.",
                KnownClaimTypes.PreferredUsername, username);

            await ProcessUserRoles(identity, jwt.EncodedToken);
        }
    }

    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        logger.LogError(context.Exception, "Entering the CustomJwtBearerEvents.AuthenticationFailed event ...");

        if (context.Exception is SecurityTokenExpiredException)
        {
            context.Response.Headers.Append("IsTokenExpired", true.ToString());
        }

        await Task.CompletedTask;
    }

    private async Task ProcessUserRoles(ClaimsIdentity identity, string jwt)
    {
        var roleNames = await userRoleService.GetMyRolesAsync(jwt);

        logger.LogInformation("I have {RolesCount} role(s).",
            roleNames.Count());

        foreach (var roleName in roleNames)
        {
            logger.LogInformation("I have Role {RoleName}.",
                roleName);

            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
        }
    }
}
