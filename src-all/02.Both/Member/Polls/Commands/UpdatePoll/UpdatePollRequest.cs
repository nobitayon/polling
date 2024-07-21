namespace Delta.Polling.Both.Member.Polls.Commands.UpdatePoll;

public class UpdatePollRequest
{
    public required Guid PollId { get; init; }
    public required string Title { get; init; }
    public required string Question { get; init; }
    public required int MaximumAnswer { get; init; }
    public required bool AllowOtherChoice { get; init; }
}

public class UpdatePollRequestValidator : AbstractValidator<UpdatePollRequest>
{
    public UpdatePollRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
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
