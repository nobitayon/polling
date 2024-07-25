using Delta.Polling.FrontEnd.Services.UserProfile;

namespace Delta.Polling.FrontEnd.Infrastructure.UserProfile.SimpleTor;

public static class ConfigureSimpleTorUserProfile
{
    public static IServiceCollection AddSimpleTorUserProfileService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<SimpleTorUserProfileOptions>(configuration.GetSection(SimpleTorUserProfileOptions.SectionKey));
        _ = services.AddTransient<IUserProfileService, SimpleTorUserProfileService>();

        return services;
    }
}
