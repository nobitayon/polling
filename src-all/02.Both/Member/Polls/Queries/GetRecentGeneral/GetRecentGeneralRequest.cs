namespace Delta.Polling.Both.Member.Polls.Queries.GetRecentGeneral;

public record GetRecentGeneralRequest : PaginatedListRequest
{
    public bool? MeAlreadyVote { get; set; }
}

public class GetRecentGeneralRequestValidator : AbstractValidator<GetRecentGeneralRequest>
{
    public GetRecentGeneralRequestValidator()
    {
        Include(new PaginatedListRequestValidator());
    }
}
