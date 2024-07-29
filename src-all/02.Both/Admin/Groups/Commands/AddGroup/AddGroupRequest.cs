namespace Delta.Polling.Both.Admin.Groups.Commands.AddGroup;

public record AddGroupRequest
{
    public required string Name { get; init; }
}

public class AddGroupRequestValidator : AbstractValidator<AddGroupRequest>
{
    public AddGroupRequestValidator()
    {
        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(GroupsMaxLengthFor.Name);
    }
}
