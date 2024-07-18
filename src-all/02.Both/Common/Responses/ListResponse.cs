namespace Delta.Polling.Both.Common.Responses;

public class ListResponse<T>
{
    public IList<T> Items { get; set; } = [];
}
