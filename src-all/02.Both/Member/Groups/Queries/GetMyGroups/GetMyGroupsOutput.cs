namespace Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;

public record GetMyGroupsOutput : Output<IEnumerable<GroupItem>>
{
}

public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
