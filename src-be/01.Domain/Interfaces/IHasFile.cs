namespace Delta.Polling.Domain.Interfaces;

public interface IHasFile : IModifiable
{
    public string FileName { get; init; }
    public long FileSize { get; init; }
    public string FileContentType { get; init; }
    public string StoredFileId { get; init; }
}
