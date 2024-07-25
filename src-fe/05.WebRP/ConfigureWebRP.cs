namespace Delta.Polling.WebRP;

public static class ConfigureWebRP
{
    public static IServiceCollection AddWebRP(this IServiceCollection services)
    {
        _ = services.AddRazorPages();
        _ = services.AddPager();

        return services;
    }
}
