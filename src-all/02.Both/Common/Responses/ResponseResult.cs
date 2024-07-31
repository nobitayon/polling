using Microsoft.AspNetCore.Mvc;

namespace Delta.Polling.Both.Common.Responses;

public record ResponseResult<T>
{
    public T? Result { get; set; }
    public ProblemDetails? Problem { get; set; }
}
