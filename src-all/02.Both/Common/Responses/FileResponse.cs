namespace Delta.Polling.Both.Common.Responses;

public class FileResponse
{
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public byte[] Content { get; set; } = [];
}
