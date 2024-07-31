using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

namespace Delta.Polling.WebRP;

public static class ConfigureWebRP
{
    public static IServiceCollection AddWebRP(this IServiceCollection services, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
    {
        _ = services.AddRazorPages();
        _ = services.AddPager();

        services.AddNotyf(config =>
        {
            config.DurationInSeconds = 7;
            config.Position = NotyfPosition.TopRight;
            config.IsDismissable = true;
            config.HasRippleEffect = true;
        });

        StaticWebAssetsLoader.UseStaticWebAssets(webHostEnvironment, configuration);

        return services;
    }
}
