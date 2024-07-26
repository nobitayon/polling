namespace Delta.Polling.Both.Member.Choices.Queries.GetChoice;

public record GetChoiceRequest
{
    public required Guid ChoiceId { get; set; }
}

public class GetChoiceRequestValidator : AbstractValidator<GetChoiceRequest>
{
    public GetChoiceRequestValidator()
    {
        _ = RuleFor(x => x.ChoiceId)
            .NotEmpty();
    }
}
