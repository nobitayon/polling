namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroup;

public record GetGroupRequest : PaginatedListRequest
{
    public required Guid GroupId { get; init; }
}

public class GetGroupRequestValidator : AbstractValidator<GetGroupRequest>
{
    public GetGroupRequestValidator()
    {
        _ = RuleFor(x => x.GroupId)
            .NotEmpty();

        Include(new PaginatedListRequestValidator());
    }
}
