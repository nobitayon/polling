using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Both.Member.Polls.Queries.GetPoll;

public record GetPollOutput : Output<PollItem>
{
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string GroupName { get; init; }
    public required string Title { get; init; }
    public required PollStatus Status { get; set; }
    public required string Question { get; init; }
    public required int MaximumAnswer { get; set; }
    public required bool AllowOtherChoice { get; set; }
    public required DateTimeOffset Created { get; set; }
    public required string CreatedBy { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public IEnumerable<ChoiceItem> ChoiceItems { get; init; } = [];
    public IEnumerable<AnswerItem> AnswerItems { get; init; } = [];
}

public record ChoiceItem
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public required bool IsOther { get; set; }
    public required bool IsChosen { get; set; }
    public required bool IsDisabled { get; set; }
    public required int NumVote { get; set; }
    public required string CreatedBy { get; init; }
}

// TODO: Apakah menggunakan list Guid saja
public record AnswerItem
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; init; } // TODO: Mungkin ini dihapus saja nanti
}

