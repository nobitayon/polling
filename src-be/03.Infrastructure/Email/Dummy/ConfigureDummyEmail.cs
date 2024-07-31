using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Dummy;

public static class ConfigureDummyEmail
{
    public static IServiceCollection AddDummyEmail(this IServiceCollection services)
    {
        _ = services.AddTransient<IEmailService, DummyEmailService>();

        return services;
    }
}
