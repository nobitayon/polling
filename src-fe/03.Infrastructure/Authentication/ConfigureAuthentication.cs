using Delta.Polling.FrontEnd.Infrastructure.Authentication.SimpleTen;

namespace Delta.Polling.FrontEnd.Infrastructure.Authentication;

public static class ConfigureAuthentication
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationOptionsSection = configuration.GetSection(AuthenticationOptions.SectionKey);
        _ = services.Configure<AuthenticationOptions>(authenticationOptionsSection);

        var authenticationOptions = authenticationOptionsSection.Get<AuthenticationOptions>()
            ?? throw new ConfigurationBindingFailedException(AuthenticationOptions.SectionKey, typeof(AuthenticationOptions));

        _ = authenticationOptions.Provider switch
        {
            AuthenticationProvider.SimpleTen => services.AddSimpleTenAuthenticationService(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(Authentication), authenticationOptions.Provider),
        };

        var logger = ConfigureLogging
            .CreateLoggerFactory()
            .CreateLogger(nameof(ConfigureAuthentication));

        logger.LogInformation("The provider for {ServiceName} service is {Provider}.",
            nameof(Authentication), authenticationOptions.Provider);

        return services;
    }
}
