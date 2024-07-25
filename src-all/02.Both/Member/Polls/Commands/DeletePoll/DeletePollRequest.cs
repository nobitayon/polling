namespace Delta.Polling.Both.Member.Polls.Commands.DeletePoll;

public record DeletePollRequest
{
    public required Guid PollId { get; init; }
}

public class DeletePollRequestValidator : AbstractValidator<DeletePollRequest>
{
    public DeletePollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();
    }
}
