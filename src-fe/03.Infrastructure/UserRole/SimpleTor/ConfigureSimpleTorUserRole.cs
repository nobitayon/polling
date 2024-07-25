using Delta.Polling.FrontEnd.Services.UserRole;

namespace Delta.Polling.FrontEnd.Infrastructure.UserRole.SimpleTor;

public static class ConfigureSimpleTorUserRole
{
    public static IServiceCollection AddSimpleTorUserRoleService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<SimpleTorUserRoleOptions>(configuration.GetSection(SimpleTorUserRoleOptions.SectionKey));
        _ = services.AddScoped<IUserRoleService, SimpleTorUserRoleService>();

        return services;
    }
}
