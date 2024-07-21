namespace Delta.Polling.Both.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;

public record AddAnotherChoiceOngoingPollOutput : Output<AddAnotherChoiceOngoingPollResult>
{
}

public record AddAnotherChoiceOngoingPollResult
{
    public required Guid ChoiceId { get; init; }
}
