using Delta.Polling.Domain.Interfaces;

namespace Delta.Polling.Domain.Abstracts;

public abstract record ModifiableEntity : CreatableEntity, IModifiable
{
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}