namespace Delta.Polling.Domain.Interfaces;

public interface IModifiable : ICreatable
{
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}
