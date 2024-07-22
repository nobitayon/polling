namespace Delta.Polling.Both.Common.Responses;

public record PaginatedListResponse<T>
{
    public required IEnumerable<T> Items { get; init; }
    public required int TotalCount { get; init; }
}
