namespace Delta.Polling.Both.Member.Choices.Queries.GetChoicesByPoll;

public record GetChoicesByPollOutput : Output<IEnumerable<ChoiceItem>>
{
}

public record ChoiceItem
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public required bool IsOther { get; set; }
}
