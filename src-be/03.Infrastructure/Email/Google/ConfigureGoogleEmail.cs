using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Google;

public static class ConfigureGoogleEmail
{
    public static IServiceCollection AddGoogleEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailGoogleSection = configuration.GetSection(GoogleEmailOptions.SectionKey);

        _ = services.Configure<GoogleEmailOptions>(emailGoogleSection);
        _ = services.AddTransient<IEmailService, GoogleEmailService>();

        return services;
    }
}
