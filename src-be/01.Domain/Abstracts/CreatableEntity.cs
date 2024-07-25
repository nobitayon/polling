namespace Delta.Polling.Domain.Abstracts;

public abstract record CreatableEntity : ICreatable
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required DateTimeOffset Created { get; init; }
    public required string CreatedBy { get; init; }
}
