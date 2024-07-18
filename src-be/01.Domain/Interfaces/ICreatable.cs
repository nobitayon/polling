namespace Delta.Polling.Domain.Interfaces;

public interface ICreatable
{
    Guid Id { get; init; }
    DateTimeOffset Created { get; init; }
    string CreatedBy { get; init; }
}