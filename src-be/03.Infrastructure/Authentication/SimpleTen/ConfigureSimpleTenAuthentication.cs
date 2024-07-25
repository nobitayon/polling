using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Delta.Polling.Infrastructure.Authentication.SimpleTen;

public static class ConfigureSimpleTenAuthentication
{
    public static IServiceCollection AddSimpleTenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var simpleTenAuthenticationOptions = configuration.GetSection(SimpleTenAuthenticationOptions.SectionKey).Get<SimpleTenAuthenticationOptions>()
            ?? throw new ConfigurationBindingFailedException(SimpleTenAuthenticationOptions.SectionKey, typeof(SimpleTenAuthenticationOptions));

        _ = services.AddScoped<CustomJwtBearerEvents>();
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = simpleTenAuthenticationOptions.AuthorityUrl;
                options.Audience = simpleTenAuthenticationOptions.Audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.EventsType = typeof(CustomJwtBearerEvents);
            });

        return services;
    }
}
