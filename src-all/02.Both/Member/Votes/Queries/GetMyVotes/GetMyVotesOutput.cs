using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;

public record GetMyVotesOutput : Output<PaginatedListResponse<VoteItem>>
{
}

public record VoteItem
{
    public required Guid Id { get; init; }
    public required Guid PollId { get; init; }
    public required string PollTitle { get; init; }
    public required string PollQuestion { get; init; }
    public required string GroupName { get; init; }
    public required PollStatus Status { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required IEnumerable<ChoiceItem> ChoiceItems { get; set; }
    public required IEnumerable<AnswerItem> AnswerItems { get; set; }
}

public record ChoiceItem
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public required int NumVote { get; init; }
}

public record AnswerItem
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
}
