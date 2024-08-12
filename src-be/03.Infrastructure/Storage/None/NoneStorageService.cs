using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.None;

public class NoneStorageService : IStorageService
{
    public Task<string> CreateAsync(byte[] content, string folderName, string fileName)
    {
        return Task.FromResult(string.Empty);
    }

    public Task DeleteAsync(string storedFileId)
    {
        return Task.FromResult(false);
    }

    public string GetUrl(string storedFileId)
    {
        return string.Empty;
    }

    public Task<byte[]> ReadAsync(string storedFileId)
    {
        return Task.FromResult(Array.Empty<byte>());
    }

    public Task<string> UpdateAsync(string storedFileId, byte[] newContent, string folderName, string fileName)
    {
        return Task.FromResult(string.Empty);
    }
}
