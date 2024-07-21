using Delta.Polling.Both.Common.Requests;

namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

public class GetGroupsRequest : PaginatedListRequest
{
}

public class GetGroupsRequestValidator : AbstractValidator<GetGroupsRequest>
{
    public GetGroupsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
