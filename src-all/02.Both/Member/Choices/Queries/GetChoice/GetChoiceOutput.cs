namespace Delta.Polling.Both.Member.Choices.Queries.GetChoice;
public record GetChoiceOutput : Output<ChoiceItem>
{
}

public record ChoiceItem
{
    public required Guid Id { get; init; }
    public required string Description { get; init; }
    public required bool IsOther { get; set; }
}
