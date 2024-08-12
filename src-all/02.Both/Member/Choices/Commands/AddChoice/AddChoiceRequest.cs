namespace Delta.Polling.Both.Member.Choices.Commands.AddChoice;

// TODO: Apa sekalian aja taro juga GroupId dalam request untuk kemudahan pengecekan
public record AddChoiceRequest : FileRequest
{
    public required Guid PollId { get; init; }
    public required string Description { get; init; }
    public IEnumerable<AddChoiceMediaRequest> MediaRequest { get; init; } = [];
}

public class AddChoiceRequestValidator : AbstractValidator<AddChoiceRequest>
{
    public AddChoiceRequestValidator()
    {
        Include(new FileRequestValidator());

        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ChoicesMaxLengthFor.Description);

        _ = RuleForEach(x => x.MediaRequest).SetValidator(new AddChoiceMediaRequestValidator());
    }
}

public record AddChoiceMediaRequest : FileRequest
{
    public required string MediaDescription { get; set; }
}

public class AddChoiceMediaRequestValidator : AbstractValidator<AddChoiceMediaRequest>
{
    public AddChoiceMediaRequestValidator()
    {
        Include(new FileRequestValidator());

        _ = RuleFor(x => x.MediaDescription)
         .NotEmpty()
         .MaximumLength(ChoicesMaxLengthFor.MediaDescription);

    }
}
