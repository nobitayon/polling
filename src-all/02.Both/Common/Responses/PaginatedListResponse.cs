namespace Delta.Polling.Both.Common.Responses;

public class PaginatedListResponse<T>
{
    public IList<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
