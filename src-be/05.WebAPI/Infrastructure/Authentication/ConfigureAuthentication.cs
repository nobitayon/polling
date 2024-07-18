using Delta.Polling.WebAPI.Infrastructure.Authentication.SimpleTen;

namespace Delta.Polling.WebAPI.Infrastructure.Authentication;

public static class ConfigureAuthentication
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationOptions = configuration.GetSection(AuthenticationOptions.SectionKey).Get<AuthenticationOptions>()
            ?? throw new ConfigurationBindingFailedException(AuthenticationOptions.SectionKey, typeof(AuthenticationOptions));

        _ = authenticationOptions.Provider switch
        {
            AuthenticationProvider.SimpleTen => services.AddSimpleTenAuthentication(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(Authentication), authenticationOptions.Provider),
        };

        return services;
    }
}
