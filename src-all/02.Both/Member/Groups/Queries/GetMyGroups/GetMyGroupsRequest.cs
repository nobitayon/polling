namespace Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;

public record GetMyGroupsRequest : PaginatedListRequest
{
}

public class GetMyGroupsRequestValidator : AbstractValidator<GetMyGroupsRequest>
{
    public GetMyGroupsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
