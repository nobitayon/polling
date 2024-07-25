namespace Delta.Polling.Both.Member.Choices.Commands.AddChoice;

// TODO: Apa sekalian aja taro juga GroupId dalam request untuk kemudahan pengecekan
public record AddChoiceRequest
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
