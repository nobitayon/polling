namespace Delta.Polling.Both.Member.Polls.Queries.GetPoll;

public record GetPollRequest
{
    public required Guid PollId { get; set; }
}

public class GetPollRequestValidator : AbstractValidator<GetPollRequest>
{
    public GetPollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
            .NotEmpty();
    }
}
