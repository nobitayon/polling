namespace Delta.Polling.Both.Admin.Groups.Commands.AddMember;

public class AddMemberRequest
{
    public required Guid GroupId { get; init; }
    public required List<string> ListMemberUsername { get; init; }
}

public class AddMemberRequestValidator : AbstractValidator<AddMemberRequest>
{
    public AddMemberRequestValidator()
    {
        _ = RuleFor(x => x.ListMemberUsername)
            .Must(x => x != null && x.Count >= 1)
           .WithMessage("ListMemberUsername must contain more than one item.");

        _ = RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
