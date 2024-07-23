using Delta.Polling.Services.UserProfile;

namespace Delta.Polling.Infrastructure.UserProfile.SimpleTor;

public static class ConfigureSimpleTorUserProfileService
{
    public static IServiceCollection AddSimpleTorUserProfileService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<SimpleTorUserProfileOptions>(configuration.GetSection(SimpleTorUserProfileOptions.SectionKey));
        _ = services.AddTransient<IUserProfileService, SimpleTorUserProfileService>();

        return services;
    }
}
