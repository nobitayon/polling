namespace Delta.Polling.Both.Member.Polls.Commands.AddPoll;

public record AddPollOutput : Output<AddPollResult>
{
}

public record AddPollResult
{
    public required Guid PollId { get; init; }
}
