namespace Delta.Polling.Infrastructure.Storage.LocalFolder;

public record LocalFolderStorageOptions
{
    public const string SectionKey = $"{nameof(Storage)}:{nameof(LocalFolder)}";

    public required string FolderPath { get; init; }
    public required string RequestPath { get; init; }
}
