namespace Delta.Polling.Both.Member.Polls.Commands.AddVote;

public record AddVoteOutput : Output<IEnumerable<AnswerItem>>
{
}

public record AnswerItem
{
    public required Guid AnswerId { get; init; }
}
