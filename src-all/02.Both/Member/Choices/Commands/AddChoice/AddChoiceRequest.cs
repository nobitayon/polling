namespace Delta.Polling.Both.Member.Choices.Commands.AddChoice;

// TODO: Apa sekalian aja taro juga GroupId dalam request untuk kemudahan pengecekan
public record AddChoiceRequest
{
    public required Guid PollId { get; init; }
    public required string Description { get; init; }
    public IEnumerable<AddChoiceMediaRequest> MediaRequest { get; init; } = [];
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

        _ = RuleFor(x => x.MediaRequest)
           .Must(mr => mr.Count() <= 5)
           .WithMessage("The number of elements in MediaRequest must be less than or equal to 5.");

        _ = RuleForEach(x => x.MediaRequest)
            .SetValidator(new AddChoiceMediaRequestValidator());
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
