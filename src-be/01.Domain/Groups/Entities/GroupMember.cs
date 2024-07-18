using Delta.Polling.Domain.Abstracts;

namespace Delta.Polling.Domain.Groups.Entities;

public record GroupMember : CreatableEntity
{
    public required Guid GroupId { get; init; }
    public Group Group { get; set; } = default!;

    public required string Username { get; init; }
}
