namespace Delta.Polling.Services.Storage;

public interface IStorageService
{
    Task<string> CreateAsync(byte[] content);
    Task<byte[]> ReadAsync(string storedFileId);
    Task DeleteAsync(string storedFileId);
    Task UpdateAsync(string storedFileId, byte[] newContent);
}
