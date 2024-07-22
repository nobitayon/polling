namespace Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;

public record GetMyGroupRequest : PaginatedListRequest
{
    public required Guid GroupId { get; set; }
}

public class GetMyGroupRequestValidator : AbstractValidator<GetMyGroupRequest>
{
    public GetMyGroupRequestValidator()
    {
        Include(new PaginatedListRequestValidator());

        _ = RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
