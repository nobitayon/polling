namespace Delta.Polling.Both.Member.Polls.Queries.GetPollWithAllAnswer;

public record GetPollWithAllAnswerOutput : Output<PollItem>
{
}

public record PollItem
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Question { get; init; }
    public IEnumerable<AnswerItem> AnswerItems { get; init; } = [];
    public IEnumerable<ChoiceItem> ChoiceItems { get; init; } = [];
}

public record AnswerItem
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; init; } // TODO: Mungkin ini dihapus saja nanti
    public required int Count { get; init; }
}

public record ChoiceItem
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; init; } // TODO: Mungkin ini dihapus saja nanti
    public required int Count { get; init; }
}
