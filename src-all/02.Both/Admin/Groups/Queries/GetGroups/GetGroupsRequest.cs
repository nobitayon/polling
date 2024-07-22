namespace Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

public record GetGroupsRequest : PaginatedListRequest
{
}

public class GetGroupsRequestValidator : AbstractValidator<GetGroupsRequest>
{
    public GetGroupsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
