namespace Delta.Polling.Domain.Interfaces;

public interface IModifiable : ICreatable
{
    DateTimeOffset? Modified { get; set; }
    string? ModifiedBy { get; set; }
}