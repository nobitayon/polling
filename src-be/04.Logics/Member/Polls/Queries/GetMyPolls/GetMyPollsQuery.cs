using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetMyPolls;
[Authorize(RoleName = RoleNameFor.Member)]
public class GetMyPollsQuery : GetMyPollsRequest, IRequest<GetMyPollsOutput>
{

}

public class GetMyPollsQueryValidator : AbstractValidator<GetMyPollsQuery>
{
    public GetMyPollsQueryValidator()
    {
        Include(new GetMyPollsRequestValidator());
    }
}

public class GetMyPollsQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyPollsQuery, GetMyPollsOutput>
{
    public async Task<GetMyPollsOutput> Handle(GetMyPollsQuery request, CancellationToken cancellationToken)
    {

        var polls = await databaseService.Polls
                        .Where(poll => poll.CreatedBy == currentUserService.Username)
                        .Select(poll => new PollItem
                        {
                            Id = poll.Id,
                            Status = poll.Status,
                            Title = poll.Title
                        }).ToListAsync(cancellationToken);

        // TODO: Membuat pagination

        return new GetMyPollsOutput { Data = polls };
    }
}
