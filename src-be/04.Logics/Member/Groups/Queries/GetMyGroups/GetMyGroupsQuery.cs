using Delta.Polling.Both.Common.Enums;
using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;

namespace Delta.Polling.Logics.Member.Groups.Queries.GetMyGroups;

[Authorize(RoleName = RoleNameFor.Member)]
public class GetMyGroupsQuery : GetMyGroupsRequest, IRequest<GetMyGroupsOutput>
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

        var groups = databaseService.GroupMembers
                        .Where(gm => gm.Username == currentUserService.Username);

        if (request.SortOrder == SortOrder.Desc)
        {
            groups = groups.Include(gm => gm.Group)
                .OrderByDescending(gm => gm.Group.Name);
        }
        else if (request.SortOrder == SortOrder.Asc)
        {
            groups = groups.Include(gm => gm.Group)
                .OrderBy(gm => gm.Group.Name);
        }

        if (request.SearchText != null)
        {
            groups = groups.Where(gm => gm.Group.Name.ToLower().Contains(request.SearchText));
        }

        groups = groups.Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        var groupItems = await groups.Select(group => new GroupItem
        {
            Id = group.Group.Id,
            Name = group.Group.Name,
        }).ToListAsync(cancellationToken);

        return new GetMyGroupsOutput { Data = groupItems };
    }
}
