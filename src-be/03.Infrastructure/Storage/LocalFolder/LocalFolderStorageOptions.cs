namespace Delta.Polling.Infrastructure.Storage.LocalFolder;

public record LocalFolderStorageOptions
{
    public const string SectionKey = $"{nameof(Storage)}:{nameof(LocalFolder)}";

    public string FolderPath { get; set; } = default!;
}
