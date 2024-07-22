namespace Delta.Polling.FrontEnd.Services.Common.Interfaces;

public interface IFile
{
    string Name { get; init; }
    byte[] Content { get; init; }
    string FileName { get; init; }
    string ContentType { get; init; }
}
