//using Azure.Storage.Blobs;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Zeta.Polling.Application.Services.Storage;

//namespace Zeta.Polling.Infrastructure.Storage.AzureBlob;

//public class AzureBlobStorageService : IStorageService
//{
//    private readonly BlobContainerClient _container;
//    private readonly ILogger<AzureBlobStorageService> _logger;

//    public string Provider => StorageProvider.AzureBlob;

//    public AzureBlobStorageService(IOptions<AzureBlobStorageOptions> azureBlobStorageOptions, ILogger<AzureBlobStorageService> logger)
//    {
//        var azureConnectionString = azureBlobStorageOptions.Value.ConnectionString;
//        var azureContainerName = azureBlobStorageOptions.Value.ContainerName;

//        _container = new BlobContainerClient(azureConnectionString, azureContainerName);
//        _logger = logger;
//    }

//    public async Task<string> CreateAsync(byte[] data)
//    {
//        var storageFileId = $"{Guid.NewGuid()}{Guid.NewGuid()}";

//        return await CreateAsync(data, storageFileId);
//    }

//    private async Task<string> CreateAsync(byte[] data, string storageFileId)
//    {
//        try
//        {
//            var isContainerExist = await _container.ExistsAsync();

//            if (!isContainerExist)
//            {
//                await _container.CreateIfNotExistsAsync();
//            }

//            var blobClient = _container.GetBlobClient(storageFileId);

//            using var dataStream = new MemoryStream(data)
//            {
//                Position = 0
//            };

//            await blobClient.UploadAsync(dataStream);

//            return storageFileId;
//        }
//        catch (Exception exception)
//        {
//            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(CreateAsync), storageFileId);

//            throw;
//        }
//    }

//    public async Task<byte[]> ReadAsync(string storageFileId)
//    {
//        try
//        {
//            var blobClient = _container.GetBlobClient(storageFileId);
//            var stream = await blobClient.OpenReadAsync();

//            using var memoryStream = new MemoryStream();
//            stream.CopyTo(memoryStream);

//            return memoryStream.ToArray();
//        }
//        catch (Exception exception)
//        {
//            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(ReadAsync), storageFileId);

//            throw;
//        }
//    }

//    public async Task<string> ReadAsBase64Async(string storageFileId)
//    {
//        var fileBytes = await ReadAsync(storageFileId);
//        var result = Convert.ToBase64String(fileBytes);
//        return result;
//    }

//    public async Task<bool> DeleteAsync(string storageFileId)
//    {
//        try
//        {
//            var blobClient = _container.GetBlobClient(storageFileId);

//            await blobClient.DeleteAsync();

//            return true;
//        }
//        catch (Exception exception)
//        {
//            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(DeleteAsync), storageFileId);

//            throw;
//        }
//    }

//    public async Task<string> UpdateAsync(string storageFileId, byte[] newData)
//    {
//        try
//        {
//            if (await DeleteAsync(storageFileId))
//            {
//                var result = await CreateAsync(newData);

//                return result;
//            }
//            else
//            {
//                var exception = new Exception($"Unable to delete file {storageFileId}");

//                _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(UpdateAsync), storageFileId);

//                return string.Empty;
//            }
//        }
//        catch (Exception exception)
//        {
//            _logger.LogError(exception, "Error in executing method {MethodName} with Storage File Id {StorageFileId}", nameof(UpdateAsync), storageFileId);

//            throw;
//        }
//    }
//}
