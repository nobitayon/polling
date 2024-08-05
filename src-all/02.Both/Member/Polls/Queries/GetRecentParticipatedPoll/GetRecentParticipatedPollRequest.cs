namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentParticipatedPoll;

public record GetRecentParticipatedPollRequest : PaginatedListRequest
{
}

public class GetRecentParticipatedPollRequestValidator : AbstractValidator<GetRecentParticipatedPollRequest>
{
    public GetRecentParticipatedPollRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
