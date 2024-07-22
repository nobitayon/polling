using Delta.Polling.Both.Admin.Groups.Queries.GetGroup;

namespace Delta.Polling.Logics.Admin.Groups.Queries.GetGroup;

[Authorize(RoleName = RoleNameFor.Administrator)]
public class GetGroupQuery : GetGroupRequest, IRequest<GetGroupOutput>
{

}

public class GetGroupQueryValidator : AbstractValidator<GetGroupQuery>
{
    public GetGroupQueryValidator()
    {
        Include(new GetGroupRequestValidator());
    }
}

public class GetGroupQueryHandler(
    IDatabaseService databaseService)
    : IRequestHandler<GetGroupQuery, GetGroupOutput>
{
    public async Task<GetGroupOutput> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        var memberGroup = await databaseService.GroupMembers
                                    .Where(gm => gm.GroupId == request.GroupId)
                                    .Select(gm => gm.Username)
                                    .ToListAsync(cancellationToken);

        var groups = await databaseService.Groups
                        .Where(g => g.Id == request.GroupId)
                        .Select(group => new GroupItem
                        {
                            Id = group.Id,
                            Name = group.Name,
                            MemberItems = memberGroup

                        }).ToListAsync(cancellationToken);

        // TODO: Pagination

        return new GetGroupOutput { Data = groups };
    }
}

