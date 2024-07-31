namespace Delta.Polling.Services.Storage;

public interface IStorageService
{
    Task<string> CreateAsync(byte[] content);
    Task<string> CreateAsync(byte[] content, string folderName, string fileName);
    Task<byte[]> ReadAsync(string storedFileId);
    Task DeleteAsync(string storedFileId);
    Task<string> UpdateAsync(string storedFileId, byte[] newContent);
    string GetUrl(string storedFileId);
}
