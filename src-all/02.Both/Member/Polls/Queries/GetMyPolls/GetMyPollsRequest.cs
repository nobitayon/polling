using Delta.Polling.Both.Common.Requests;

namespace Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

public class GetMyPollsRequest : PaginatedListRequest
{
}

public class GetMyPollsRequestValidator : AbstractValidator<GetMyPollsRequest>
{
    public GetMyPollsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
