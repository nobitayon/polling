using Delta.Polling.Domain.Abstracts;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Domain.Groups.Entities;

public record Group : CreatableEntity
{
    public required string Name { get; set; }

    public List<Poll> Polls { get; init; } = [];
    public List<GroupMember> GroupMembers { get; init; } = [];
}
