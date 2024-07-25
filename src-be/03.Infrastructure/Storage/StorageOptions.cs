namespace Delta.Polling.Infrastructure.Storage;

public record StorageOptions
{
    public const string SectionKey = nameof(Storage);

    public required string Provider { get; init; }
}

public static class StorageProvider
{
    public const string None = nameof(None);
    public const string LocalFolder = nameof(LocalFolder);
    public const string AzureBlob = nameof(AzureBlob);
}
