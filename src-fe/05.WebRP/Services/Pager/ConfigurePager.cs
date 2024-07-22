namespace Delta.Polling.WebRP.Services.Pager;

public static class ConfigurePager
{
    public static IServiceCollection AddPager(this IServiceCollection services)
    {
        _ = services.AddSingleton<PagerService>();

        return services;
    }
}
