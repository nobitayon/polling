namespace Delta.Polling.WebRP.Infrastructure.Authorization;

public static class ConfigureAuthorizationService
{
    public static IServiceCollection AddAuthorizationService(this IServiceCollection services)
    {
        _ = services.AddAuthorization(options =>
        {
            options.AddPolicy(RoleNameFor.Contributor, policy =>
            {
                _ = policy.RequireRole(RoleNameFor.Contributor);
            });

            options.AddPolicy(RoleNameFor.Administrator, policy =>
            {
                _ = policy.RequireRole(RoleNameFor.Administrator);
            });

            options.AddPolicy(RoleNameFor.Member, policy =>
            {
                _ = policy.RequireRole(RoleNameFor.Member);
            });
        });

        _ = services.AddRazorPages(options =>
        {
            _ = options.Conventions.AuthorizeFolder($"/{RoleNameFor.Contributor}", RoleNameFor.Contributor);
            _ = options.Conventions.AuthorizeFolder($"/{RoleNameFor.Administrator}", RoleNameFor.Administrator);
            _ = options.Conventions.AuthorizeFolder($"/{RoleNameFor.Member}", RoleNameFor.Member);
        });

        return services;
    }
}
