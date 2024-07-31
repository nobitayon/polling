using Delta.Polling.Services.UserProfile;

namespace Delta.Polling.Infrastructure.UserProfile.SimpleTor;

public static class ConfigureSimpleTorUserProfile
{
    public static IServiceCollection AddSimpleTorUserProfile(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<SimpleTorUserProfileOptions>(configuration.GetSection(SimpleTorUserProfileOptions.SectionKey));
        _ = services.AddScoped<IUserProfileService, SimpleTorUserProfileService>();

        return services;
    }
}
