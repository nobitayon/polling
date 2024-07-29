namespace Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

public record GetUsersNotMemberFromGroupOutput : Output<PaginatedListResponse<MemberItem>>
{
}

public record MemberItem
{
    public required string Username { get; init; }
}
