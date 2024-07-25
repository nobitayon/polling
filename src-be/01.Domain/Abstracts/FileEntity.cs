namespace Delta.Polling.Domain.Abstracts;

public abstract record FileEntity : ModifiableEntity, IHasFile
{
    public required string FileName { get; init; }
    public required string FileContentType { get; init; }
    public required long FileSize { get; init; }
    public required string StoredFileId { get; init; }
}
