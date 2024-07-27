namespace Delta.Polling.Both.Member.Polls.Commands.FinishPoll;
public record FinishPollRequest
{
    public required Guid PollId { get; init; }
}

public class FinishPollRequestValidator : AbstractValidator<FinishPollRequest>
{
    public FinishPollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();
    }
}
