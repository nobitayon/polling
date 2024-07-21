namespace Delta.Polling.Both.Member.Polls.Commands.StartPoll;

public class StartPollRequest
{
    public required Guid PollId { get; init; }
}

public class StartPollRequestValidator : AbstractValidator<StartPollRequest>
{
    public StartPollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();
    }
}
