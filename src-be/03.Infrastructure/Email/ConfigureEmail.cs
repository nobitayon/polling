using Delta.Polling.Infrastructure.Email.Dummy;
using Delta.Polling.Infrastructure.Email.Ethereal;

namespace Delta.Polling.Infrastructure.Email;

public static class ConfigureEmail
{
    public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        var emailOptionsSection = configuration.GetSection(EmailOptions.SectionKey);
        _ = services.Configure<EmailOptions>(emailOptionsSection);

        var emailOptions = emailOptionsSection.Get<EmailOptions>()
            ?? throw new ConfigurationBindingFailedException(EmailOptions.SectionKey, typeof(EmailOptions));

        _ = emailOptions.Provider switch
        {
            EmailProvider.Ethereal => services.AddEtherealEmail(configuration),
            _ => services.AddDummyEmail()
        };

        var logger = ConfigureLogging
            .CreateLoggerFactory()
            .CreateLogger(nameof(ConfigureEmail));

        logger.LogInformation("The provider for {ServiceName} service is {Provider}.",
            nameof(Email), emailOptions.Provider);

        return services;
    }
}
