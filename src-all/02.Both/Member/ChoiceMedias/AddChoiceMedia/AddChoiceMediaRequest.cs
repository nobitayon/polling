namespace Delta.Polling.Both.Member.ChoiceMedias.AddChoiceMedia;

public abstract record AddChoiceMediaRequest : FileRequest
{
    public required Guid ChoiceId { get; init; }
    public required string Description { get; set; }
}

public class AddChoiceMediaRequestValidator : AbstractValidator<AddChoiceMediaRequest>
{
    public AddChoiceMediaRequestValidator()
    {
        Include(new FileRequestValidator());

        _ = RuleFor(x => x.ChoiceId)
         .NotEmpty();

        _ = RuleFor(x => x.Description)
         .NotEmpty()
         .MaximumLength(ChoicesMaxLengthFor.Description);

    }
}
