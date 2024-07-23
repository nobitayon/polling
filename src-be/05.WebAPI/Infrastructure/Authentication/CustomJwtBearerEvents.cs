using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Delta.Polling.WebAPI.Infrastructure.Authentication;

public class CustomJwtBearerEvents(ILogger<CustomJwtBearerEvents> logger)
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
        logger.LogInformation("Entering the CustomJwtBearerEvents.Challenge event ...");

        return Task.CompletedTask;
    }

    public override async Task TokenValidated(TokenValidatedContext context)
    {
        logger.LogInformation("Entering the CustomJwtBearerEvents.TokenValidated event ...");

        var jwt = context.SecurityToken as JsonWebToken;

        if (jwt is not null)
        {
            var principal = context.Principal!;
            var identity = (principal.Identity as ClaimsIdentity)!;

            identity.AddClaim(new Claim(CustomClaimTypes.AccessToken, jwt.EncodedToken));
        }

        await Task.CompletedTask;
    }

    public override async Task AuthenticationFailed(AuthenticationFailedContext context)
    {
        logger.LogError(context.Exception, "Entering the CustomJwtBearerEvents.AuthenticationFailed event ...");

        await Task.CompletedTask;
    }
}
