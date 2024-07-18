using Polling.WebRP.Infrastructure.Authentication.SimpleTen;

namespace Delta.Polling.WebRP.Infrastructure.Authentication;

public static class ConfigureAuthenticationService
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var authenticationSection = configuration.GetSection(AuthenticationOptions.SectionKey);
        var authenticationOptions = authenticationSection.Get<AuthenticationOptions>()
            ?? throw new ConfigurationBindingFailedException(AuthenticationOptions.SectionKey, typeof(AuthenticationOptions));

        _ = services.Configure<AuthenticationOptions>(authenticationSection);
        _ = services.AddScoped<CustomOpenIdConnectEvents>();

        _ = authenticationOptions.Provider switch
        {
            AuthenticationProvider.SimpleTen => services.AddSimpleTenAuthenticationService(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(Authentication), authenticationOptions.Provider),
        };

        return services;
    }
}
