namespace Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;

public record GetOngoingPollsRequest : PaginatedListRequest
{
}

public class GetOngoingPollsRequestValidator : AbstractValidator<GetOngoingPollsRequest>
{
    public GetOngoingPollsRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
