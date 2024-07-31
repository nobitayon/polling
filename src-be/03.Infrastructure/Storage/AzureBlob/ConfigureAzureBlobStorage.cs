using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.AzureBlob;

public static class ConfigureAzureBlobStorage
{
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AzureBlobStorageOptions>(configuration.GetSection(AzureBlobStorageOptions.SectionKey));
        _ = services.AddSingleton<IStorageService, AzureBlobStorageService>();

        return services;
    }
}
