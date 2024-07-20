namespace Delta.Polling.Both.Member.Polls.Commands.AddPoll;

public class AddPollRequest
{
    public required Guid GroupId { get; init; }
    public required string Title { get; init; }
    public required string Question { get; init; }
    public required int MaximumAnswer { get; init; }
    public required bool AllowOtherChoice { get; init; }
}

public class AddPollRequestValidator : AbstractValidator<AddPollRequest>
{
    public AddPollRequestValidator()
    {
        _ = RuleFor(x => x.GroupId)
           .NotEmpty();

        _ = RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(PollsMaxLengthFor.Title);

        _ = RuleFor(x => x.Question)
            .NotEmpty()
            .MaximumLength(PollsMaxLengthFor.Question);

        _ = RuleFor(x => x.MaximumAnswer)
            .InclusiveBetween(PollsMinValueFor.MaximumAnswer, PollsMaxValueFor.MaximumAnswer);
    }
}
