using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

public record GetMyPollsOutput : Output<PaginatedListResponse<PollItem>>
{
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Question { get; init; }
    public required PollStatus Status { get; set; }
    public required string GroupName { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required DateTimeOffset? Modified { get; init; }
}
