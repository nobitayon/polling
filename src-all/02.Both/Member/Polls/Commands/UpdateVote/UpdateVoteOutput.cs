namespace Delta.Polling.Both.Member.Polls.Commands.UpdateVote;

public record UpdateVoteOutput : Output<IEnumerable<AnswerItem>>
{
}

public record AnswerItem
{
    public required Guid AnswerId { get; init; }
}
