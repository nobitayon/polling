namespace Delta.Polling.Both.Member.Choices.Commands.AddChoice;

public class AddChoiceRequest
{
    public required Guid PollId { get; init; }
    public required string Description { get; init; }
}

public class AddChoiceRequestValidator : AbstractValidator<AddChoiceRequest>
{
    public AddChoiceRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ChoicesMaxLengthFor.Description);
    }
}
