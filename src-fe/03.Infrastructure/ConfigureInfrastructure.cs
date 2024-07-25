using Delta.Polling.FrontEnd.Infrastructure.Authentication;
using Delta.Polling.FrontEnd.Infrastructure.Authorization;
using Delta.Polling.FrontEnd.Infrastructure.BackEnd;
using Delta.Polling.FrontEnd.Infrastructure.CurrentUser;
using Delta.Polling.FrontEnd.Infrastructure.Logging;
using Delta.Polling.FrontEnd.Infrastructure.UserProfile;
using Delta.Polling.FrontEnd.Infrastructure.UserRole;

namespace Delta.Polling.FrontEnd.Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostBuilder hostBuilder, IConfiguration configuration)
    {
        _ = hostBuilder.AddLogging();
        _ = services.AddAuthenticationService(configuration);
        _ = services.AddAuthorizationService();
        _ = services.AddBackEnd(configuration);
        _ = services.AddCurrentUser();
        _ = services.AddUserProfile(configuration);
        _ = services.AddUserRole(configuration);

        return services;
    }
}
