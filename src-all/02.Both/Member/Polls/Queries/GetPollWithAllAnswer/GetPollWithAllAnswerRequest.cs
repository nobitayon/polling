namespace Delta.Polling.Both.Member.Polls.Queries.GetPollWithAllAnswer;

public record GetPollWithAllAnswerRequest
{
    public required Guid PollId { get; set; }
}

public class GetPollWithAllAnswerRequestValidator : AbstractValidator<GetPollWithAllAnswerRequest>
{
    public GetPollWithAllAnswerRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
            .NotEmpty();
    }
}
