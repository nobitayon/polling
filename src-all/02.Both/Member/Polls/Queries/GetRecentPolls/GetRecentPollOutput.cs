using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentPolls;

public record GetRecentPollsOutput : Output<PaginatedListResponse<PollItem>>
{
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string GroupName { get; init; }
    public required string Title { get; init; }
    public required PollStatus Status { get; set; }
    public required bool IsVotedByMe { get; init; }
    public required string CreatedBy { get; set; }
    public required DateTimeOffset Created { get; set; }
}
