using Delta.Polling.Infrastructure.UserProfile.SimpleTor;

namespace Delta.Polling.Infrastructure.UserProfile;

public static class ConfigureUserProfileService
{
    public static IServiceCollection AddUserProfileService(this IServiceCollection services, IConfiguration configuration)
    {
        var userProfileOptions = configuration.GetSection(UserProfileOptions.SectionKey).Get<UserProfileOptions>()
            ?? throw new ConfigurationBindingFailedException(UserProfileOptions.SectionKey, typeof(UserProfileOptions));

        _ = userProfileOptions.Provider switch
        {
            UserProfileProvider.SimpleTor => services.AddSimpleTorUserProfileService(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(UserProfile), userProfileOptions.Provider),
        };

        return services;
    }
}
