using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;
using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.Logics.Member.Groups.Queries.GetMyGroups;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyGroupsQuery : GetMyGroupsRequest, IRequest<GetMyGroupsOutput>
{

}

public class GetMyGroupsQueryValidator : AbstractValidator<GetMyGroupsQuery>
{
    public GetMyGroupsQueryValidator()
    {
        Include(new GetMyGroupsRequestValidator());
    }
}

public class GetMyGroupsQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyGroupsQuery, GetMyGroupsOutput>
{
    public async Task<GetMyGroupsOutput> Handle(GetMyGroupsQuery request, CancellationToken cancellationToken)
    {
        var query = databaseService.GroupMembers
            .AsNoTracking()
            .Where(gm => gm.Username == currentUserService.Username);

        var totalCount = await query.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.Include(gm => gm.Group)
                        .OrderBy(gm => gm.Group.Name);
        }
        else
        {
            var sortOrder = request.SortOrder is not null
                ? request.SortOrder.Value
                : SortOrder.Asc;

            if (sortOrder is SortOrder.Asc)
            {
                if (request.SortField == nameof(GroupItem.Name))
                {
                    query = query.Include(gm => gm.Group)
                        .OrderBy(gm => gm.Group.Name);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(PollItem.Title))
                {
                    query = query.Include(gm => gm.Group)
                        .OrderByDescending(gm => gm.Group.Name);
                }
            }
            else
            {
                query = query.Include(gm => gm.Group)
                        .OrderBy(gm => gm.Group.Name);
            }
        }

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var groups = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(gm => new GroupItem
            {
                Id = gm.GroupId,
                Name = gm.Group.Name
            })
            .ToListAsync(cancellationToken);

        var output = new GetMyGroupsOutput
        {
            Data = new PaginatedListResponse<GroupItem>
            {
                Items = groups,
                TotalCount = totalCount
            }
        };

        return output;
    }
}
