using Delta.Polling.Infrastructure.Authentication;
using Delta.Polling.Infrastructure.CurrentUser;
using Delta.Polling.Infrastructure.Database;
using Delta.Polling.Infrastructure.Documentation;
using Delta.Polling.Infrastructure.Email;
using Delta.Polling.Infrastructure.Logging;
using Delta.Polling.Infrastructure.Storage;
using Delta.Polling.Infrastructure.UserProfile;
using Delta.Polling.Infrastructure.UserRole;

namespace Delta.Polling.Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostBuilder hostBuilder, IConfiguration configuration)
    {
        _ = hostBuilder.AddLogging();
        _ = services.AddAuthenticationService(configuration);
        _ = services.AddCurrentUser();
        _ = services.AddDatabase(configuration);
        _ = services.AddDocumentation(configuration);
        _ = services.AddEmail(configuration);
        _ = services.AddStorage(configuration);
        _ = services.AddUserProfile(configuration);
        _ = services.AddUserRole(configuration);

        return services;
    }
}
