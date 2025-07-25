﻿namespace Delta.Polling.Both.Member.Polls.Commands.UpdateVote;

public record UpdateVoteRequest
{
    public required Guid PollId { get; init; }
    public required IEnumerable<Guid> ListChoice { get; set; }
}

public class UpdateVoteRequestValidator : AbstractValidator<UpdateVoteRequest>
{
    public UpdateVoteRequestValidator()
    {
        _ = RuleFor(x => x.PollId)
           .NotEmpty();

        _ = RuleFor(x => x.ListChoice)
           .Must(x => x != null && x.Count() >= 1)
           .WithMessage("ListChoice must contain more than one item.");
    }
}
