namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

public record GetGroupsOutput : Output<IEnumerable<GroupItem>>
{
}

public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}

