using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.LocalFolder;

public static class ConfigureLocalFolderStorage
{
    public static IServiceCollection AddLocalFolderStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<LocalFolderStorageOptions>(configuration.GetSection(LocalFolderStorageOptions.SectionKey));
        _ = services.AddTransient<IStorageService, LocalFolderStorageService>();

        return services;
    }
}
