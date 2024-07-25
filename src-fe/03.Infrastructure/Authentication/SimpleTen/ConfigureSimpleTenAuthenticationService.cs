using Delta.Polling.FrontEnd.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Delta.Polling.FrontEnd.Infrastructure.Authentication.SimpleTen;

public static class ConfigureSimpleTenAuthenticationService
{
    public static IServiceCollection AddSimpleTenAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        var simpleTenAuthenticationOptions = configuration.GetSection(SimpleTenAuthenticationOptions.SectionKey).Get<SimpleTenAuthenticationOptions>()
            ?? throw new ConfigurationBindingFailedException(SimpleTenAuthenticationOptions.SectionKey, typeof(SimpleTenAuthenticationOptions));

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

            options.Authority = simpleTenAuthenticationOptions.Authority;
            options.ClientId = simpleTenAuthenticationOptions.ClientId;
            options.ClientSecret = simpleTenAuthenticationOptions.ClientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.EventsType = typeof(CustomOpenIdConnectEvents);

            foreach (var scope in simpleTenAuthenticationOptions.Scopes)
            {
                options.Scope.Add(scope);
            }

            options.TokenValidationParameters = new()
            {
                NameClaimType = CustomClaimTypes.Name
            };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        return services;
    }
}
