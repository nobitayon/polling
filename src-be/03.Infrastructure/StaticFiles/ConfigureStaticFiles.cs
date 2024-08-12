using Delta.Polling.Infrastructure.Storage;
using Delta.Polling.Infrastructure.Storage.LocalFolder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Delta.Polling.Infrastructure.StaticFiles;

public static class ConfigureStaticFiles
{
    public static void UseStaticFiles(this IApplicationBuilder app, IConfiguration configuration)
    {
        var storageOptions = configuration.GetSection(StorageOptions.SectionKey).Get<StorageOptions>()
            ?? throw new ConfigurationBindingFailedException(StorageOptions.SectionKey, typeof(StorageOptions));

        if (storageOptions.Provider is StorageProvider.LocalFolder)
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

            var logger = ConfigureLogging
                .CreateLoggerFactory()
                .CreateLogger(nameof(ConfigureStorage));

            logger.LogInformation("The application serves {ServiceName} in {FolderPath} as relative path {RequestPath}.",
                "Static Files", localFolderStorageOptions.FolderPath, localFolderStorageOptions.RequestPath);
        }
    }
}
