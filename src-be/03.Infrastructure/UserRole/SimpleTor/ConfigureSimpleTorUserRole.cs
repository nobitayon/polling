using Delta.Polling.Services.UserRole;

namespace Delta.Polling.Infrastructure.UserRole.SimpleTor;

public static class ConfigureSimpleTorUserRole
{
    public static IServiceCollection AddSimpleTorUserRole(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<SimpleTorUserRoleOptions>(configuration.GetSection(SimpleTorUserRoleOptions.SectionKey));
        _ = services.AddScoped<IUserRoleService, SimpleTorUserRoleService>();

        return services;
    }
}
