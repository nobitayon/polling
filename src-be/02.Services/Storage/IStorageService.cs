namespace Delta.Polling.Services.Storage;

public interface IStorageService
{
    Task<string> CreateAsync(byte[] content, string folderName, string fileName);
    Task<byte[]> ReadAsync(string storedFileId);
    Task DeleteAsync(string storedFileId);
    Task<string> UpdateAsync(string storedFileId, byte[] newContent, string folderName, string fileName);
    string GetUrl(string storedFileId);
}
