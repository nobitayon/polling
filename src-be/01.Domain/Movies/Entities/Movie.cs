using Delta.Polling.Base.Movies.Enums;
using Delta.Polling.Domain.Abstracts;

namespace Delta.Polling.Domain.Movies.Entities;

public record Movie : ModifiableEntity
{
    public DateTimeOffset? Approved { get; set; }
    public string? ApprovedBy { get; set; }
    public required string Title { get; set; }
    public required string Storyline { get; set; }
    public required decimal Budget { get; set; }
    public required MovieStatus Status { get; set; }
}
