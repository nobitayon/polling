using System.Net;
using RestSharp;

namespace Delta.Polling.FrontEnd.Services.BackEnd;

public interface IBackEndService
{
    Task<ResponseResult<T>> SendRequestAsync<T>(RestRequest restRequest, CancellationToken cancellationToken = default);
}

public record ResponseResult<T>
{
    public T? Result { get; set; }
    public ErrorResponse? Error { get; set; }
}

public record NoContentResponse
{
}

public record ErrorResponse
{
    public required string Type { get; set; }
    public required string Title { get; set; }
    public required HttpStatusCode Status { get; set; }
    public required string Detail { get; set; }
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

    public string StatusDisplayText => $"{(int)Status} {Status}";
}