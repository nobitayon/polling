namespace Delta.Polling.Domain.Interfaces;

public interface ICreatable
{
    public Guid Id { get; init; }
    public DateTimeOffset Created { get; init; }
    public string CreatedBy { get; init; }
}
