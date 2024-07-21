using Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

namespace Delta.Polling.Logics.Admin.Groups.Queries.GetGroups;

[Authorize(RoleName = RoleNameFor.Administrator)]
public class GetGroupsQuery : GetGroupsRequest, IRequest<GetGroupsOutput>
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
        var groups = await databaseService.Groups
                        .Select(group => new GroupItem
                        {
                            Id = group.Id,
                            Name = group.Name,
                        }).ToListAsync(cancellationToken);

        // TODO: Pagination

        return new GetGroupsOutput { Data = groups };
    }
}

