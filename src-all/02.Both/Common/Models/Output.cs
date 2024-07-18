namespace Delta.Polling.Both.Common.Models;

public abstract record Output<T> where T : class
{
    public required T Data { get; init; }
}
