using Delta.Polling.Services.UserRole;

namespace Delta.Polling.Infrastructure.UserRole.SimpleTor;

public static class ConfigureSimpleTorUserRoleService
{
    public static IServiceCollection AddSimpleTorUserRoleService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<SimpleTorUserRoleOptions>(configuration.GetSection(SimpleTorUserRoleOptions.SectionKey));
        _ = services.AddScoped<IUserRoleService, SimpleTorUserRoleService>();

        return services;
    }
}
