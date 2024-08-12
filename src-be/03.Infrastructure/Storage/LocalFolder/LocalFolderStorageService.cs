using Delta.Polling.Services.Storage;
using Microsoft.AspNetCore.Http;

namespace Delta.Polling.Infrastructure.Storage.LocalFolder;

public class LocalFolderStorageService(
    IOptions<LocalFolderStorageOptions> localFolderStorageOptions,
    IHttpContextAccessor httpContextAccessor)
    : IStorageService
{
    private readonly string _folderPath = localFolderStorageOptions.Value.FolderPath;

    public async Task<string> CreateAsync(byte[] content, string folderName, string fileName)
    {
        var directory = Directory.CreateDirectory(Path.Combine(_folderPath, folderName));

        var filePath = Path.Combine(directory.FullName, fileName);
        using var fileStream = File.Create(filePath);
        await fileStream.WriteAsync(content.AsMemory(0, content.Length));

        var storedFileId = Path.Combine(folderName, fileName).Replace('\\', '/');

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

    public string GetUrl(string storedFileId)
    {
        var request = httpContextAccessor.HttpContext!.Request;
        var requestPath = localFolderStorageOptions.Value.RequestPath;

        return $"{request.Scheme}://{request.Host}{request.PathBase}{requestPath}/{storedFileId}";
    }

    public Task<byte[]> ReadAsync(string storedFileId)
    {
        return File.ReadAllBytesAsync(Path.Combine(_folderPath, storedFileId));
    }

    public async Task<string> UpdateAsync(string storedFileId, byte[] newContent, string folderName, string fileName)
    {
        await DeleteAsync(storedFileId);

        return await CreateAsync(newContent, folderName, fileName);
    }
}
