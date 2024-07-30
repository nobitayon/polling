namespace Delta.Polling.Both.Admin.Groups.Commands.AddMember;

public record AddMemberRequest
{
    public required Guid GroupId { get; init; }
    public required string Username { get; init; }
}

public class AddMemberRequestValidator : AbstractValidator<AddMemberRequest>
{
    public AddMemberRequestValidator()
    {
        _ = RuleFor(x => x.Username)
            .NotEmpty();

        _ = RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
