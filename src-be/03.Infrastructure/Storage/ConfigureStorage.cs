using Delta.Polling.Infrastructure.Logging;
using Delta.Polling.Infrastructure.Storage.LocalFolder;
using Delta.Polling.Infrastructure.Storage.None;

namespace Delta.Polling.Infrastructure.Storage;

public static class ConfigureStorage
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var storageOptions = configuration.GetSection(StorageOptions.SectionKey).Get<StorageOptions>()
            ?? throw new ConfigurationBindingFailedException(StorageOptions.SectionKey, typeof(StorageOptions));

        _ = storageOptions.Provider switch
        {
            StorageProvider.None => services.AddNoneStorageService(),
            StorageProvider.LocalFolder => services.AddLocalFolderStorageService(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(Storage), storageOptions.Provider),
        };

        var logger = configuration
            .CreateLoggerFactory()
            .CreateLogger(nameof(ConfigureStorage));

        logger.LogInformation("Using {Provider} for {ServiceName} service.",
            storageOptions.Provider, nameof(Storage));

        return services;
    }
}
