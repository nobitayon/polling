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
            .NotNull()
            .When(x => x.MediaRequest.Count() > 0)
            .DependentRules(() =>
            {
                _ = RuleFor(x => x.MediaRequest)
                    .ForEach(mediaRequest =>
                        mediaRequest.SetValidator(new AddChoiceMediaRequestValidator())
                    )
                    .WithMessage("Each media request must be valid.");
            });
    }
}

public record AddChoiceMediaRequest : FileRequest
{
    public required string Description { get; set; }
}

public class AddChoiceMediaRequestValidator : AbstractValidator<AddChoiceMediaRequest>
{
    public AddChoiceMediaRequestValidator()
    {
        Include(new FileRequestValidator());

        _ = RuleFor(x => x.Description)
         .NotEmpty()
         .MaximumLength(ChoicesMaxLengthFor.Description);

    }
}
