using Delta.Polling.Services.CurrentUser;

namespace Delta.Polling.WebAPI.Infrastructure.CurrentUser;

public static class ConfigureCurrentUserService
{
    public static void AddCurrentUserService(this IServiceCollection services)
    {
        _ = services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
