namespace Delta.Polling.Both.Admin.Groups.Commands.RemoveMember;

public class RemoveMemberRequest
{
    public required Guid GroupId { get; init; }
    public required List<string> ListMemberUsername { get; init; }
}

public class RemoveMemberRequestValidator : AbstractValidator<RemoveMemberRequest>
{
    public RemoveMemberRequestValidator()
    {
        _ = RuleFor(x => x.ListMemberUsername)
            .Must(x => x != null && x.Count >= 1)
           .WithMessage("ListMemberUsername must contain more than one item.");

        _ = RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
