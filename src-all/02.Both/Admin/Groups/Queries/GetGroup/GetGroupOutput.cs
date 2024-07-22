namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroup;

public record GetGroupOutput : Output<IEnumerable<GroupItem>>
{
}

public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public IEnumerable<string> MemberItems { get; init; } = [];
}
