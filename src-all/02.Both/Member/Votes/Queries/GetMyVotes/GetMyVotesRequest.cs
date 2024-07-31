namespace Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;

public record GetMyVotesRequest : PaginatedListRequest
{
}

public class GetMyVotesRequestValidator : AbstractValidator<GetMyVotesRequest>
{
    public GetMyVotesRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
