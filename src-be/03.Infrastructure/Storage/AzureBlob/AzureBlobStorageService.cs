using Azure.Storage.Blobs;
using Delta.Polling.Services.Storage;

namespace Delta.Polling.Infrastructure.Storage.AzureBlob;

public class AzureBlobStorageService(
    IOptions<AzureBlobStorageOptions> azureBlobStorageOptions,
    ILogger<AzureBlobStorageService> logger)
    : IStorageService
{
    private readonly string _accountName = azureBlobStorageOptions.Value.AccountName;
    private readonly string _containerName = azureBlobStorageOptions.Value.ContainerName;
    private readonly BlobContainerClient _blobContainerClient = new(azureBlobStorageOptions.Value.ConnectionString, azureBlobStorageOptions.Value.ContainerName);

    public async Task<string> CreateAsync(byte[] content, string folderName, string fileName)
    {
        try
        {
            var isContainerExist = await _blobContainerClient.ExistsAsync();

            if (!isContainerExist)
            {
                _ = await _blobContainerClient.CreateIfNotExistsAsync();
            }

            var storedFileId = $"{folderName}/{fileName}";
            var blobClient = _blobContainerClient.GetBlobClient(storedFileId);

            using var dataStream = new MemoryStream(content)
            {
                Position = 0
            };

            _ = await blobClient.UploadAsync(dataStream);

            return storedFileId;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to upload content to container {ContainerName} in Azure Blob Storage",
                _containerName);

            throw;
        }
    }

    public async Task DeleteAsync(string storedFileId)
    {
        try
        {
            var blobClient = _blobContainerClient.GetBlobClient(storedFileId);

            _ = await blobClient.DeleteAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to delete content {StoredFileId} in container {ContainerName} in Azure Blob Storage",
                storedFileId, _containerName);
            throw;
        }
    }

    public string GetUrl(string storedFileId)
    {
        return $"https://{_accountName}.blob.core.windows.net/{_containerName}/{storedFileId}";
    }

    public async Task<byte[]> ReadAsync(string storedFileId)
    {
        try
        {
            var blobClient = _blobContainerClient.GetBlobClient(storedFileId);
            var stream = await blobClient.OpenReadAsync();

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to read content {StoredFileId} from container {ContainerName} in Azure Blob Storage",
                storedFileId, _containerName);

            throw;
        }
    }

    public async Task<string> UpdateAsync(string storedFileId, byte[] newContent, string folderName, string fileName)
    {
        try
        {
            await DeleteAsync(storedFileId);

            return await CreateAsync(newContent, folderName, fileName);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to update content {StoredFileId} in container {ContainerName} in Azure Blob Storage",
                storedFileId, _containerName);

            throw;
        }
    }
}
