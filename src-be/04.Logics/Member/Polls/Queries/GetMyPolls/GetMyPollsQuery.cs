using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetMyPolls;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyPollsQuery : GetMyPollsRequest, IRequest<GetMyPollsOutput>
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

        //var polls = await databaseService.Polls
        //                .Where(poll => poll.CreatedBy == currentUserService.Username)
        //                .Select(poll => new PollItem
        //                {
        //                    Id = poll.Id,
        //                    Status = poll.Status,
        //                    Title = poll.Title
        //                }).ToListAsync(cancellationToken);

        var myGroup = await databaseService.GroupMembers
                        .Include(gm => gm.Group)
                        .Where(gm => gm.Username == currentUserService.Username)
                        .Select(gm => gm.Group.Id)
                        .ToListAsync(cancellationToken);

        var query = databaseService.Polls
            .AsNoTracking()
            .Where(p => p.CreatedBy == currentUserService.Username && myGroup.Contains(p.GroupId));

        var totalCount = await query.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.OrderBy(poll => poll.Title);
        }
        else
        {
            var sortOrder = request.SortOrder is not null
                ? request.SortOrder.Value
                : SortOrder.Asc;

            if (sortOrder is SortOrder.Asc)
            {
                if (request.SortField == nameof(PollItem.Title))
                {
                    query = query.OrderBy(poll => poll.Title);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(PollItem.Title))
                {
                    query = query.OrderByDescending(poll => poll.Title);
                }
            }
            else
            {
                query = query.OrderBy(poll => poll.Title);
            }
        }

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var polls = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(poll => new PollItem
            {
                Id = poll.Id,
                Title = poll.Title,
                Status = poll.Status
            })
            .ToListAsync(cancellationToken);

        var output = new GetMyPollsOutput
        {
            Data = new PaginatedListResponse<PollItem>
            {
                Items = polls,
                TotalCount = totalCount
            }
        };

        return output;
    }
}
