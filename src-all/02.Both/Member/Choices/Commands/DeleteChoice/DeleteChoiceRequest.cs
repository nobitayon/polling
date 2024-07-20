namespace Delta.Polling.Both.Member.Choices.Commands.DeleteChoice;

public class DeleteChoiceRequest
{
    public required Guid PollId { get; init; }
    public required Guid ChoiceId { get; init; }
}

public class DeleteChoiceRequestValidator : AbstractValidator<DeleteChoiceRequest>
{
    public DeleteChoiceRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.ChoiceId)
          .NotEmpty();
    }
}
