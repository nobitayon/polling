using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Delta.Polling.WebAPI.Infrastructure.Authorization;

public static class ConfigureAuthorization
{
    public static void AddAuthorizationService(this IServiceCollection services)
    {
        _ = services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();
        });
    }
}
