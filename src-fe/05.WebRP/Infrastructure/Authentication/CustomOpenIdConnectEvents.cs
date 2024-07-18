using System.Security.Claims;
using Delta.Polling.FrontEnd.Services.CurrentUser.Statics;
using Delta.Polling.FrontEnd.Services.UserProfile;
using Delta.Polling.FrontEnd.Services.UserRole;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Delta.Polling.WebRP.Infrastructure.Authentication;

public class CustomOpenIdConnectEvents(
    IUserProfileService userProfileService,
    IUserRoleService userRoleService)
    : OpenIdConnectEvents
{
    public override async Task TokenValidated(TokenValidatedContext context)
    {
        var principal = context.Principal!;
        var identity = (principal.Identity as ClaimsIdentity)!;
        var username = principal.FindFirstValue(JwtClaimTypes.PreferredUserName)!;

        if (context.TokenEndpointResponse is not null)
        {
            Console.WriteLine($"AccessToken: {context.TokenEndpointResponse.AccessToken}");

            identity.AddClaim(new Claim(ClaimTypeFor.AccessToken, context.TokenEndpointResponse.AccessToken));
        }

        TransformJwtClaims(identity, username);
        await ProcessUserProfile(identity, username);
        await ProcessUserRoles(identity, username);
    }

    private static void TransformJwtClaims(ClaimsIdentity identity, string username)
    {
        identity.AddClaim(new Claim(ClaimTypes.Name, username));
        identity.RemoveClaim(identity.FindFirst(JwtClaimTypes.PreferredUserName));
    }

    private async Task ProcessUserProfile(ClaimsIdentity identity, string username)
    {
        var userProfileItem = await userProfileService.GetUserProfileAsync(username);

        identity.AddClaim(new Claim(ClaimTypeFor.Name, userProfileItem.Name));
        identity.AddClaim(new Claim(ClaimTypeFor.Email, userProfileItem.Email));
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
