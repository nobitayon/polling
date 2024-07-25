namespace Delta.Polling.Domain.Interfaces;

public interface IHasFile : IModifiable
{
    string FileName { get; init; }
    long FileSize { get; init; }
    string FileContentType { get; init; }
    string StoredFileId { get; init; }
}
