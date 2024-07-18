using Delta.Polling.FrontEnd.Services.BackEnd;

namespace Delta.Polling.FrontEnd.Infrastructure.BackEnd;

public static class ConfigureBackEnd
{
    public static void AddBackEndService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<BackEndOptions>(configuration.GetSection(BackEndOptions.SectionKey));
        _ = services.AddScoped<IBackEndService, BackEndService>();
    }
}
