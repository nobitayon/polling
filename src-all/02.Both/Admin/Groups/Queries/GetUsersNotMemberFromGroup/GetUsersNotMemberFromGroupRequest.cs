namespace Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

public record GetUsersNotMemberFromGroupRequest : PaginatedListRequest
{
    public required Guid GroupId { get; init; }
}

public class GetUsersNotMemberFromGroupRequestValidator : AbstractValidator<GetUsersNotMemberFromGroupRequest>
{
    public GetUsersNotMemberFromGroupRequestValidator()
    {
        _ = RuleFor(x => x.GroupId)
            .NotEmpty();

        Include(new PaginatedListRequestValidator());
    }
}
