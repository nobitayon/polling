using Delta.Polling.Services.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Delta.Polling.Infrastructure.Storage.LocalFolder;

public static class ConfigureLocalFolderStorage
{
    public static IServiceCollection AddLocalFolderStorage(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<LocalFolderStorageOptions>(configuration.GetSection(LocalFolderStorageOptions.SectionKey));
        _ = services.AddTransient<IStorageService, LocalFolderStorageService>();

        return services;
    }

    public static IApplicationBuilder UseLocalFolderStorage(this IApplicationBuilder app, IConfiguration configuration)
    {
        var localFolderStorageOptions = configuration.GetSection(LocalFolderStorageOptions.SectionKey).Get<LocalFolderStorageOptions>()
            ?? throw new ConfigurationBindingFailedException(LocalFolderStorageOptions.SectionKey, typeof(LocalFolderStorageOptions));

        _ = app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(localFolderStorageOptions.FolderPath),
            RequestPath = localFolderStorageOptions.RequestPath,
            OnPrepareResponse = responseContext => responseContext.Context.Response.Headers
                .Append("Cache-Control", $"public, max-age={60 * 60 * 24 * 7}")
        });

        return app;
    }
}
