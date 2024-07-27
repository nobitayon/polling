namespace Delta.Polling.Both.Member.Choices.Commands.DeleteChoice;

public record DeleteChoiceRequest
{
    public required Guid ChoiceId { get; init; }
}

public class DeleteChoiceRequestValidator : AbstractValidator<DeleteChoiceRequest>
{
    public DeleteChoiceRequestValidator()
    {
        _ = RuleFor(x => x.ChoiceId)
          .NotEmpty();
    }
}
