using Microsoft.AspNetCore.Mvc;

namespace Delta.Polling.Both.Common.Responses;

public abstract record FileResponse
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Content { get; init; }

    public FileContentResult ToFileContentResult()
    {
        return new FileContentResult(Content, ContentType)
        {
            FileDownloadName = FileName
        };
    }
}
