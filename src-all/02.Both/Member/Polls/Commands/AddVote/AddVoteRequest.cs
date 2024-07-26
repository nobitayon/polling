namespace Delta.Polling.Both.Member.Polls.Commands.AddVote;
public record AddVoteRequest
{
    public required Guid PollId { get; init; }
    public required IEnumerable<Guid> ListChoice { get; set; }
}

public class AddVoteRequestValidator : AbstractValidator<AddVoteRequest>
{
    public AddVoteRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.ListChoice)
           .Must(x => x != null && x.Count() >= 1)
           .WithMessage("ListChoice must contain more than one item.");
    }
}
