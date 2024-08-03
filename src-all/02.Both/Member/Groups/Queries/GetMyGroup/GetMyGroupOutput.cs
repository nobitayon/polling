using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;

public record GetMyGroupOutput : Output<GroupItem>
{
}

// TODO: apakah Id, Name, Created, CreatedBy dijadiin satu field 
public record GroupItem
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required string CreatedBy { get; init; }
    public required PaginatedListResponse<PollItem> PollItems { get; init; }
    public IEnumerable<string> MemberItems { get; init; } = [];
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string GroupName { get; init; }
    public required string Title { get; init; }
    public required PollStatus Status { get; set; }
    public required DateTimeOffset Created { get; set; }
    public required string CreatedBy { get; set; }
}
