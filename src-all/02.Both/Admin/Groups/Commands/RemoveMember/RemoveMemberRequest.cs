namespace Delta.Polling.Both.Admin.Groups.Commands.RemoveMember;

public record RemoveMemberRequest
{
    public required Guid GroupId { get; init; }
    public required string Username { get; init; }
}

public class RemoveMemberRequestValidator : AbstractValidator<RemoveMemberRequest>
{
    public RemoveMemberRequestValidator()
    {
        _ = RuleFor(x => x.Username)
            .NotEmpty();

        _ = RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
