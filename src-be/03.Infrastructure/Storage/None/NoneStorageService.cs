using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.None;

public class NoneStorageService : IStorageService
{
    public Task<string> CreateAsync(byte[] content)
    {
        return Task.FromResult(string.Empty);
    }

    public Task<string> CreateAsync(byte[] content, string folderName, string fileName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string storedFileId)
    {
        return Task.FromResult(false);
    }

    public string GetUrl(string storedFileId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> ReadAsync(string storedFileId)
    {
        return Task.FromResult(Array.Empty<byte>());
    }

    public Task<string> UpdateAsync(string storedFileId, byte[] newContent)
    {
        return Task.FromResult(string.Empty);
    }
}
