using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;

public record GetOngoingPollsOutput : Output<PaginatedListResponse<PollItem>>
{
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required PollStatus Status { get; set; }
}
