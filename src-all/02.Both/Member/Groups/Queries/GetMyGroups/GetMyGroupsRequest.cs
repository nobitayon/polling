using Delta.Polling.Both.Common.Requests;

namespace Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;

public class GetMyGroupsRequest : PaginatedListRequest
{
}

public class GetMyGroupsRequestValidator : AbstractValidator<GetMyGroupsRequest>
{
    public GetMyGroupsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
