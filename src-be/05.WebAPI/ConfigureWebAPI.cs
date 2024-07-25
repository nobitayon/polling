using Delta.Polling.WebAPI.Filters;

namespace Delta.Polling.WebAPI;

public static class ConfigureWebAPI
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        _ = services.AddControllers(options => _ = options.Filters.Add<CustomExceptionFilterAttribute>());

        return services;
    }
}
