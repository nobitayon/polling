using Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

namespace Delta.Polling.Logics.Admin.Groups.Queries.GetGroups;

[Authorize(RoleName = RoleNameFor.Admin)]
public record GetGroupsQuery : GetGroupsRequest, IRequest<GetGroupsOutput>
{

}

public class GetGroupsQueryValidator : AbstractValidator<GetGroupsQuery>
{
    public GetGroupsQueryValidator()
    {
        Include(new GetGroupsRequestValidator());
    }
}

public class GetGroupsQueryHandler(
    IDatabaseService databaseService)
    : IRequestHandler<GetGroupsQuery, GetGroupsOutput>
{
    public async Task<GetGroupsOutput> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        var query = databaseService.Groups
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.OrderBy(group => group.Name);
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
                    query = query.OrderBy(group => group.Name);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(GroupItem.Name))
                {
                    query = query.OrderByDescending(group => group.Name);
                }
            }
            else
            {
                query = query.OrderBy(poll => poll.Name);
            }
        }

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var groups = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(group => new GroupItem
            {
                Id = group.Id,
                Name = group.Name
            })
            .ToListAsync(cancellationToken);

        var output = new GetGroupsOutput
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

