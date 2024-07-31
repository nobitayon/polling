using Delta.Polling.Infrastructure.Storage.AzureBlob;
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
            StorageProvider.None => services.AddNoneStorage(),
            StorageProvider.AzureBlob => services.AddAzureBlobStorage(configuration),
            StorageProvider.LocalFolder => services.AddLocalFolderStorage(configuration),
            _ => throw new UnsupportedServiceProviderException(nameof(Storage), storageOptions.Provider),
        };

        var logger = ConfigureLogging
            .CreateLoggerFactory()
            .CreateLogger(nameof(ConfigureStorage));

        logger.LogInformation("The provider for {ServiceName} service is {Provider}.",
            nameof(Storage), storageOptions.Provider);

        return services;
    }
}
