namespace Delta.Polling.Both.Member.Choices.Commands.UpdateChoice;

public record UpdateChoiceRequest
{
    public required Guid PollId { get; init; }
    public required Guid ChoiceId { get; init; }
    public required string Description { get; set; }
}

public class UpdateChoiceRequestValidator : AbstractValidator<UpdateChoiceRequest>
{
    public UpdateChoiceRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.ChoiceId)
          .NotEmpty();

        _ = RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ChoicesMaxLengthFor.Description);
    }
}
