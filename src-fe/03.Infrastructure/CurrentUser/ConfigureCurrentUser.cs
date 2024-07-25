using Delta.Polling.FrontEnd.Services.CurrentUser;

namespace Delta.Polling.FrontEnd.Infrastructure.CurrentUser;

public static class ConfigureCurrentUser
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        _ = services.AddHttpContextAccessor();
        _ = services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
