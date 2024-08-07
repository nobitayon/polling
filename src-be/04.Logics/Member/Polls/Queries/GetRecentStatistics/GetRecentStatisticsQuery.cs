using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetRecentStatistics;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetRecentStatistics;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetRecentStatisticsQuery : GetRecentStatisticsRequest, IRequest<GetRecentStatisticsOutput>
{

}

public class GetRecentStatisticsQueryValidator : AbstractValidator<GetRecentStatisticsQuery>
{
    public GetRecentStatisticsQueryValidator()
    {
        Include(new GetRecentStatisticsRequestValidator());
    }
}

public class GetRecentStatisticsQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetRecentStatisticsQuery, GetRecentStatisticsOutput>
{
    public async Task<GetRecentStatisticsOutput> Handle(GetRecentStatisticsQuery request, CancellationToken cancellationToken)
    {

        var groupIamIn = await databaseService.GroupMembers
                           .Where(gm => gm.Username == currentUserService.Username)
                           .Select(gm => gm.GroupId)
                           .ToListAsync(cancellationToken);

        var numParticipatedPoll = await databaseService.Voters
                                    .Where(v => v.Username == currentUserService.Username)
                                    .CountAsync(cancellationToken);

        var numActivePollButNotParticipate = await databaseService.Polls
                                                    .Include(p => p.Voters)
                                                    .Where(p => p.Status == PollStatus.Ongoing
                                                                && !p.Voters.Any(v => v.Username == currentUserService.Username)
                                                                && groupIamIn.Contains(p.GroupId))
                                                    .CountAsync(cancellationToken);

        return new GetRecentStatisticsOutput
        {
            Data = new StatisticsUser
            {
                NumActivePollNotParticipated = numActivePollButNotParticipate,
                NumParticipatedPoll = numParticipatedPoll
            }
        };
    }
}

