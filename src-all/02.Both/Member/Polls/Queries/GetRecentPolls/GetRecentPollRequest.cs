namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentPolls;

public record GetRecentPollsRequest : PaginatedListRequest
{
}

public class GetRecentPollsRequestValidator : AbstractValidator<GetRecentPollsRequest>
{
    public GetRecentPollsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
