using Delta.Polling.Both.Configurables;

namespace Delta.Polling.FrontEnd.Infrastructure.Authorization;

public static class ConfigureAuthorization
{
    public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
    {
        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(RoleNameFor.Contributor, policy => _ = policy.RequireRole(RoleNameFor.Contributor));
            options.AddPolicy(RoleNameFor.Moderator, policy => _ = policy.RequireRole(RoleNameFor.Moderator));
            options.AddPolicy(RoleNameFor.Member, policy => _ = policy.RequireRole(RoleNameFor.Member));
        });

        _ = services.AddRazorPages(options =>
        {
            _ = options.Conventions.AuthorizeFolder($"/{RoleNameFor.Contributor}", RoleNameFor.Contributor);
            _ = options.Conventions.AuthorizeFolder($"/{RoleNameFor.Moderator}", RoleNameFor.Moderator);
            _ = options.Conventions.AuthorizeFolder($"/{RoleNameFor.Member}", RoleNameFor.Member);
        });

        return services;
    }
}
