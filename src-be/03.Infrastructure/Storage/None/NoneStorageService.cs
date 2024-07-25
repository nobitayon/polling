using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.None;

public class NoneStorageService : IStorageService
{
    public NoneStorageService(ILogger<NoneStorageService> logger)
    {
        // TODO: Ganti jadi di ConfigureStorageService aja
        logger.LogWarning("{ServiceName} service is set to {ServiceProvider}.", nameof(Storage), StorageProvider.None);
    }

    public Task<string> CreateAsync(byte[] content)
    {
        return Task.FromResult(string.Empty);
    }

    public Task DeleteAsync(string storedFileId)
    {
        return Task.FromResult(false);
    }

    public Task<byte[]> ReadAsync(string storedFileId)
    {
        return Task.FromResult(Array.Empty<byte>());
    }

    public Task UpdateAsync(string storedFileId, byte[] newContent)
    {
        return Task.FromResult(string.Empty);
    }
}
