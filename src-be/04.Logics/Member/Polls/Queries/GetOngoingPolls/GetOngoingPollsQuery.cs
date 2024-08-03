using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetOngoingPolls;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetOngoingPollsQuery : GetOngoingPollsRequest, IRequest<GetOngoingPollsOutput>
{

}

public class GetOngoingPollsQueryValidator : AbstractValidator<GetOngoingPollsQuery>
{
    public GetOngoingPollsQueryValidator()
    {
        Include(new GetOngoingPollsRequestValidator());
    }
}

public class GetOngoingPollsQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetOngoingPollsQuery, GetOngoingPollsOutput>
{
    public async Task<GetOngoingPollsOutput> Handle(GetOngoingPollsQuery request, CancellationToken cancellationToken)
    {
        var myGroup = await databaseService.GroupMembers
                        .Include(gm => gm.Group)
                        .Where(gm => gm.Username == currentUserService.Username)
                        .Select(gm => gm.Group.Id)
                        .ToListAsync(cancellationToken);

        var query = databaseService.Polls
            .AsNoTracking()
            .Where(p => p.Status == PollStatus.Ongoing && myGroup.Contains(p.GroupId));

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
                else if (request.SortField == nameof(Poll.Created))
                {
                    query = query.OrderBy(poll => poll.Created);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(PollItem.Title))
                {
                    query = query.OrderByDescending(poll => poll.Title);
                }
                else if (request.SortField == nameof(Poll.Created))
                {
                    query = query.OrderByDescending(poll => poll.Created);
                }
            }
            else
            {
                query = query.OrderBy(poll => poll.Created);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchField) && !string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (request.SearchField == nameof(Poll.Title))
            {
                query = query.Where(poll => poll.Title.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Question))
            {
                query = query.Where(poll => poll.Question.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Group.Name))
            {
                query = query.Where(poll => poll.Group.Name.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Status))
            {
                if (request.SearchText == nameof(PollStatus.Draft))
                {
                    query = query.Where(poll => poll.Status == PollStatus.Draft);
                }
                else if (request.SearchText == nameof(PollStatus.Ongoing))
                {
                    query = query.Where(poll => poll.Status == PollStatus.Ongoing);
                }
                else if (request.SearchText == nameof(PollStatus.Finished))
                {
                    query = query.Where(poll => poll.Status == PollStatus.Finished);
                }
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var polls = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(poll => new PollItem
            {
                Id = poll.Id,
                Title = poll.Title,
                Status = poll.Status,
                Created = poll.Created,
                CreatedBy = poll.CreatedBy,
                GroupName = poll.Group.Name
            })
            .ToListAsync(cancellationToken);

        var output = new GetOngoingPollsOutput
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
