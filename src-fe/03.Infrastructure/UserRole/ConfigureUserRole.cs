using Delta.Polling.FrontEnd.Infrastructure.UserRole.SimpleTor;

namespace Delta.Polling.FrontEnd.Infrastructure.UserRole;

public static class ConfigureUserRole
{
    public static IServiceCollection AddUserRole(this IServiceCollection services, IConfiguration configuration)
    {
        var userRoleOptions = configuration.GetSection(UserRoleOptions.SectionKey).Get<UserRoleOptions>()
            ?? throw new ConfigurationBindingFailedException(UserRoleOptions.SectionKey, typeof(UserRoleOptions));

        _ = userRoleOptions.Provider switch
        {
            UserRoleProvider.SimpleTor => services.AddSimpleTorUserRoleService(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(UserRole), userRoleOptions.Provider),
        };

        return services;
    }
}
