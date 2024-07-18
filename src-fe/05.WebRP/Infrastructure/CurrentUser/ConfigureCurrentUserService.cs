using Delta.Polling.FrontEnd.Services.CurrentUser;

namespace Delta.Polling.WebRP.Infrastructure.CurrentUser;

public static class ConfigureCurrentUserService
{
    public static void AddCurrentUserService(this IServiceCollection services)
    {
        _ = services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
