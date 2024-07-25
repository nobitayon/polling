using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.LocalFolder;

public class LocalFolderStorageService(IOptions<LocalFolderStorageOptions> localFolderStorageOptions)
    : IStorageService
{
    private readonly string _folderPath = localFolderStorageOptions.Value.FolderPath;

    public async Task<string> CreateAsync(byte[] content)
    {
        var storedFileId = $"{Guid.NewGuid()}{Guid.NewGuid()}";
        var filePath = Path.Combine(_folderPath, storedFileId);

        using var fileStream = File.Create(filePath);
        await fileStream.WriteAsync(content.AsMemory(0, content.Length));

        return storedFileId;
    }

    public Task DeleteAsync(string storedFileId)
    {
        var filePath = Path.Combine(_folderPath, storedFileId);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    public Task<byte[]> ReadAsync(string storedFileId)
    {
        return File.ReadAllBytesAsync(Path.Combine(_folderPath, storedFileId));
    }

    public async Task UpdateAsync(string storedFileId, byte[] newContent)
    {
        await DeleteAsync(storedFileId);

        _ = await CreateAsync(newContent);
    }
}
