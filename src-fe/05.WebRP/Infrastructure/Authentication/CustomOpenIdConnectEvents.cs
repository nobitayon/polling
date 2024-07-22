using System.Security.Claims;
using Delta.Polling.FrontEnd.Services.CurrentUser.Statics;
using Delta.Polling.FrontEnd.Services.UserProfile;
using Delta.Polling.FrontEnd.Services.UserRole;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Delta.Polling.WebRP.Infrastructure.Authentication;

public class CustomOpenIdConnectEvents(
    IUserProfileService userProfileService,
    IUserRoleService userRoleService,
    ILogger<CustomOpenIdConnectEvents> logger)
    : OpenIdConnectEvents
{
    public override Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.AuthorizationCodeReceived ...");

        return Task.CompletedTask;
    }

    public override Task AccessDenied(AccessDeniedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.AccessDenied ...");

        return Task.CompletedTask;
    }

    public override Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.AuthenticationFailed ...");

        return Task.CompletedTask;
    }

    public override Task MessageReceived(MessageReceivedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.MessageReceived ...");

        return Task.CompletedTask;
    }

    public override Task RedirectToIdentityProvider(RedirectContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.RedirectToIdentityProvider ...");

        return Task.CompletedTask;
    }

    public override Task RemoteFailure(RemoteFailureContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.RemoteFailure ...");

        return Task.CompletedTask;
    }

    public override Task TicketReceived(TicketReceivedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.TicketReceived ...");

        return Task.CompletedTask;
    }

    public override Task TokenResponseReceived(TokenResponseReceivedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.TokenResponseReceived ...");

        return Task.CompletedTask;
    }

    public override Task UserInformationReceived(UserInformationReceivedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.UserInformationReceived ...");

        return Task.CompletedTask;
    }

    public override Task RedirectToIdentityProviderForSignOut(RedirectContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.RedirectToIdentityProviderForSignOut ...");

        return Task.CompletedTask;
    }

    public override Task RemoteSignOut(RemoteSignOutContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.RemoteSignOut ...");

        return Task.CompletedTask;
    }

    public override Task SignedOutCallbackRedirect(RemoteSignOutContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.SignedOutCallbackRedirect ...");

        return Task.CompletedTask;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        logger.LogInformation("Entering CustomOpenIdConnectEvents.TokenValidated ...");

        var principal = context.Principal!;
        var identity = (principal.Identity as ClaimsIdentity)!;

        if (context.TokenEndpointResponse is not null)
        {
            var jwt = context.TokenEndpointResponse.AccessToken;

            logger.LogInformation("TokenValidated JWT: {Jwt}", jwt);

            identity.AddClaim(new Claim(ClaimTypeFor.AccessToken, context.TokenEndpointResponse.AccessToken));

            await ProcessUserProfile(identity, jwt);
            await ProcessUserRoles(identity, jwt);
        }
    }

    private async Task ProcessUserProfile(ClaimsIdentity identity, string jwt)
    {
        var userProfileItem = await userProfileService.GetMyProfileAsync(jwt);

        identity.AddClaim(new Claim(ClaimTypeFor.Name, userProfileItem.Name));
        identity.AddClaim(new Claim(ClaimTypeFor.Email, userProfileItem.Email));
    }

    private async Task ProcessUserRoles(ClaimsIdentity identity, string jwt)
    {
        var roleNames = await userRoleService.GetMyRolesAsync(jwt);

        foreach (var roleName in roleNames)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
        }
    }
}
