namespace Delta.Polling.Services.Storage;

public interface IStorageService
{
    public Task<string> CreateAsync(byte[] content);
    public Task<string> CreateAsync(byte[] content, string folderName, string fileName);
    public Task<byte[]> ReadAsync(string storedFileId);
    public Task DeleteAsync(string storedFileId);
    public Task<string> UpdateAsync(string storedFileId, byte[] newContent);
    public string GetUrl(string storedFileId);
}
