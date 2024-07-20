using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetPoll;

public record GetPollOutput : Output<PollItem>
{
}

public record PollItem
{
    public required string Title { get; init; }
    public required PollStatus Status { get; set; }
    public required string Question { get; init; }
    public required int MaximumAnswer { get; set; }
    public required bool AllowOtherChoice { get; set; }
    public IEnumerable<ChoiceItem> ChoiceItems { get; init; } = [];
    public IEnumerable<AnswerItem> AnswerItems { get; init; } = [];
}

public record ChoiceItem
{
    public required string Description { get; init; }
    public required bool IsOther { get; set; }
}

// TODO: Apakah menggunakan list Guid saja
public record AnswerItem
{
    public required Guid ChoiceId { get; init; }
}

