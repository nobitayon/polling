namespace Delta.Polling.Both.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;

public class AddAnotherChoiceOngoingPollRequest
{
    public required Guid PollId { get; init; }
    public required string Description { get; init; }
}

public class AddAnotherChoiceOngoingPollRequestValidator : AbstractValidator<AddAnotherChoiceOngoingPollRequest>
{
    public AddAnotherChoiceOngoingPollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(ChoicesMaxLengthFor.Description);
    }
}
