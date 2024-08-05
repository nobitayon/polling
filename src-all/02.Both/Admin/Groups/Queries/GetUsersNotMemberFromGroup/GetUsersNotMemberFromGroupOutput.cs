namespace Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

public record GetUsersNotMemberFromGroupOutput : Output<GetUsersNotMemberFromGroupResult>
{
}

public record GetUsersNotMemberFromGroupResult
{
    public required GroupItem GroupItem { get; init; }
    public required PaginatedListResponse<MemberItem> MemberItems { get; init; }
}

public record MemberItem
{
    public required string Username { get; init; }
}

public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
