using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Microsoft;

public static class ConfigureMicrosoftEmail
{
    public static IServiceCollection AddMicrosoftEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailMicrosoftSection = configuration.GetSection(MicrosoftEmailOptions.SectionKey);

        _ = services.Configure<MicrosoftEmailOptions>(emailMicrosoftSection);
        _ = services.AddTransient<IEmailService, MicrosoftEmailService>();

        return services;
    }
}
