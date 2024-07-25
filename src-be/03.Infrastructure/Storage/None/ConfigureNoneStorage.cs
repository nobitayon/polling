using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.None;

public static class ConfigureNoneStorage
{
    public static IServiceCollection AddNoneStorageService(this IServiceCollection services)
    {
        _ = services.AddSingleton<IStorageService, NoneStorageService>();

        return services;
    }
}
