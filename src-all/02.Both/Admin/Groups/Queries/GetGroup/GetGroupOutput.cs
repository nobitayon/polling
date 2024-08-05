namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroup;

public record GetGroupOutput : Output<GetGroupResult>
{
}

public record GetGroupResult
{
    public required GroupItem GroupItem { get; init; }
    public required PaginatedListResponse<MemberItem> MemberItems { get; init; }
}

public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int NumOngoingPoll { get; init; }
}

public record MemberItem
{
    public required Guid GroupMemberId { get; init; }
    public required string Username { get; init; }
    public required int NumOngoingPoll { get; init; }
}
