using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentParticipatedPoll;

public record GetRecentParticipatedPollOutput : Output<IEnumerable<PollItem>>
{
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string GroupName { get; set; } = default!;
    public required string Title { get; set; } = default!;
    public required string Question { get; set; } = default!;
    public required PollStatus Status { get; set; } = default!;
    public IEnumerable<AnswerItem> AnswerItems { get; set; } = [];
    public IEnumerable<ChoiceItem> WinnerAnswers { get; set; } = [];
    public required DateTimeOffset Created { get; set; } = default!;
    public required string CreatedBy { get; set; } = default!;
    public required DateTimeOffset? Modified { get; set; } = default!;
    public required string? ModifiedBy { get; set; } = default!;
}

public record AnswerItem
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; init; }
}

public record ChoiceItem
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; init; }
    public required int NumVote { get; init; }
}
