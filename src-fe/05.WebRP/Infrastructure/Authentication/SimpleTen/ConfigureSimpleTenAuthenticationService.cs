using Delta.Polling.FrontEnd.Services.CurrentUser.Statics;
using Delta.Polling.WebRP.Infrastructure.Authentication;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Polling.WebRP.Infrastructure.Authentication.SimpleTen;

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
            options.ResponseType = OidcConstants.ResponseTypes.Code;
            options.EventsType = typeof(CustomOpenIdConnectEvents);

            foreach (var scope in simpleTenAuthenticationOptions.Scopes)
            {
                options.Scope.Add(scope);
            }

            options.TokenValidationParameters = new()
            {
                NameClaimType = ClaimTypeFor.Name,
            };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

        return services;
    }
}
