using Delta.Polling.Infrastructure.Email.Dummy;
using Delta.Polling.Infrastructure.Email.Google;
using Delta.Polling.Infrastructure.Email.Microsoft;

namespace Delta.Polling.Infrastructure.Email;

public static class ConfigureEmail
{
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailOptions = configuration.GetSection(EmailOptions.SectionKey).Get<EmailOptions>()
            ?? throw new ConfigurationBindingFailedException(EmailOptions.SectionKey, typeof(EmailOptions));

        _ = emailOptions.Provider switch
        {
            EmailProvider.Google => services.AddGoogleEmailService(configuration),
            EmailProvider.Microsoft => services.AddMicrosoftEmailService(configuration),
            _ => services.AddDummyEmailService()
        };

        return services;
    }
}