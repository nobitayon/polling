using Delta.Polling.Infrastructure.UserProfile.SimpleTor;

namespace Delta.Polling.Infrastructure.UserProfile;

public static class ConfigureUserProfile
{
    public static IServiceCollection AddUserProfile(this IServiceCollection services, IConfiguration configuration)
    {
        var userProfileOptions = configuration.GetSection(UserProfileOptions.SectionKey).Get<UserProfileOptions>()
            ?? throw new ConfigurationBindingFailedException(UserProfileOptions.SectionKey, typeof(UserProfileOptions));

        _ = userProfileOptions.Provider switch
        {
            UserProfileProvider.SimpleTor => services.AddSimpleTorUserProfile(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(UserProfile), userProfileOptions.Provider),
        };

        var logger = ConfigureLogging
            .CreateLoggerFactory()
            .CreateLogger(nameof(ConfigureUserProfile));

        logger.LogInformation("The provider for {ServiceName} service is {Provider}.",
            nameof(UserProfile), userProfileOptions.Provider);

        return services;
    }
}
