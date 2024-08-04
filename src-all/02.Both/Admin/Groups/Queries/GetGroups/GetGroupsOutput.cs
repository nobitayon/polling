namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

public record GetGroupsOutput : Output<PaginatedListResponse<GroupItem>>
{
}

public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string CreatedBy { get; init; }
    public required DateTimeOffset Created { get; init; }
}

