using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;
using Delta.Polling.Domain.Groups.Entities;

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
            Console.WriteLine($"Yang kebaca di logic {sortOrder}");
            if (sortOrder is SortOrder.Asc)
            {
                Console.WriteLine($"Yang kebaca di logic ASC {sortOrder}");
                if (request.SortField == nameof(Group.Name))
                {
                    query = query.Include(gm => gm.Group)
                        .OrderBy(gm => gm.Group.Name);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                Console.WriteLine($"Yang kebaca di logic DESC {sortOrder}");
                if (request.SortField == nameof(Group.Name))
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

        if (!string.IsNullOrWhiteSpace(request.SearchField) && !string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (request.SearchField == nameof(Group.Name))
            {
                query = query.Where(gm => gm.Group.Name.ToLower().Contains(request.SearchText!.ToLower()));
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

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
