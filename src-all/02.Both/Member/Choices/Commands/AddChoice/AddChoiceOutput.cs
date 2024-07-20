namespace Delta.Polling.Both.Member.Choices.Commands.AddChoice;

public record AddChoiceOutput : Output<AddChoiceResult>
{
}

public record AddChoiceResult
{
    public required Guid ChoiceId { get; init; }
}
