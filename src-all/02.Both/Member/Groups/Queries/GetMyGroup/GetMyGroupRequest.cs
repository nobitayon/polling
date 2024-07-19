using Delta.Polling.Both.Common.Requests;

namespace Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;

public class GetMyGroupRequest : PaginatedListRequest
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
