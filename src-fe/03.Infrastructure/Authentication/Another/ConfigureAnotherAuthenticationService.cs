using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Delta.Polling.FrontEnd.Infrastructure.Authentication.Another;

public static class ConfigureAnotherAuthenticationService
{
    public static IServiceCollection AddAnotherAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var anotherAuthenticationOptions = configuration.GetSection(AnotherAuthenticationOptions.SectionKey).Get<AnotherAuthenticationOptions>()
            ?? throw new ConfigurationBindingFailedException(AnotherAuthenticationOptions.SectionKey, typeof(AnotherAuthenticationOptions));

        _ = services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.SaveTokens = true;
            options.UseTokenLifetime = true;

            options.Authority = anotherAuthenticationOptions.Authority;
            options.ClientId = anotherAuthenticationOptions.ClientId;
            options.ClientSecret = anotherAuthenticationOptions.ClientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.EventsType = typeof(CustomOpenIdConnectEvents);

            foreach (var scope in anotherAuthenticationOptions.Scopes)
            {
                options.Scope.Add(scope);
            }

            options.TokenValidationParameters = new()
            {
                NameClaimType = CustomClaimTypes.Name
            };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        _ = services.AddScoped<CustomOpenIdConnectEvents>();

        return services;
    }
}
