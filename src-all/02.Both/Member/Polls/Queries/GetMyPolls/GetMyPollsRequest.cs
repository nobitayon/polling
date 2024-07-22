namespace Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

public record GetMyPollsRequest : PaginatedListRequest
{
}

public class GetMyPollsRequestValidator : AbstractValidator<GetMyPollsRequest>
{
    public GetMyPollsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
