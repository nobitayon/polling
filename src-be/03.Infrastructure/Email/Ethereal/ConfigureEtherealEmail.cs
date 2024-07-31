using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Ethereal;

public static class ConfigureEtherealEmail
{
    public static IServiceCollection AddEtherealEmail(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<EtherealEmailOptions>(configuration.GetSection(EtherealEmailOptions.SectionKey));
        _ = services.AddTransient<IEmailService, EtherealEmailService>();

        return services;
    }
}
