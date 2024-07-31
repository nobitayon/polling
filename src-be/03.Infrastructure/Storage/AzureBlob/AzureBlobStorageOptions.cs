namespace Delta.Polling.Infrastructure.Storage.AzureBlob;

public class AzureBlobStorageOptions
{
    public static readonly string SectionKey = $"{nameof(Storage)}:{nameof(AzureBlob)}";

    public required string ConnectionString { get; init; }
    public required string AccountName { get; init; }
    public required string ContainerName { get; init; }
}
