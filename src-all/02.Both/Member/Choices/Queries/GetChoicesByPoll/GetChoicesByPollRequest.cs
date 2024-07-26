namespace Delta.Polling.Both.Member.Choices.Queries.GetChoicesByPoll;

public record GetChoicesByPollRequest
{
    public required Guid PollId { get; set; }
}

public class GetChoicesByPollRequestValidator : AbstractValidator<GetChoicesByPollRequest>
{
    public GetChoicesByPollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
            .NotEmpty();
    }
}
