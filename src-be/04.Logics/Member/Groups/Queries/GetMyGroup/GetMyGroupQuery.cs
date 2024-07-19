using Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;

namespace Delta.Polling.Logics.Member.Groups.Queries.GetMyGroup;

[Authorize(RoleName = RoleNameFor.Member)]
public class GetMyGroupQuery : GetMyGroupRequest, IRequest<GetMyGroupOutput>
{

}

public class GetMyGroupQueryValidator : AbstractValidator<GetMyGroupQuery>
{
    public GetMyGroupQueryValidator()
    {
        Include(new GetMyGroupRequestValidator());
    }
}

public class GetMyGroupQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyGroupQuery, GetMyGroupOutput>
{
    public async Task<GetMyGroupOutput> Handle(GetMyGroupQuery request, CancellationToken cancellationToken)
    {

        var memberGroup = await databaseService.GroupMembers
                        .Where(gm => gm.GroupId == request.GroupId)
                        .Select(gm => gm.Username)
                        .ToListAsync(cancellationToken);

        var isInGroup = memberGroup
            .Any(member =>
            {
                return member == currentUserService.Username;
            });

        if (!isInGroup)
        {
            throw new Exception($"You cannot access group with Id {request.GroupId} because you are not member of group");
        }

        var groupDetails = await databaseService.Groups
                            .SingleOrDefaultAsync(group => group.Id == request.GroupId, cancellationToken)
                            ?? throw new EntityNotFoundException("Group", request.GroupId);

        // TODO: Belum ada pagination untuk yang ini. Mungkin nunggu kak fu hari senin
        var listPolling = await databaseService.Polls
                            .Where(poll => poll.GroupId == request.GroupId)
                            .Select(poll => new PollItem
                            {
                                Id = poll.Id,
                                Title = poll.Title,
                                Status = poll.Status
                            }).ToListAsync(cancellationToken);

        var result = new GroupItem
        {
            Id = groupDetails.Id,
            Name = groupDetails.Name,
            Created = groupDetails.Created,
            CreatedBy = groupDetails.CreatedBy,
            MemberItems = memberGroup,
            PollItems = listPolling
        };

        return new GetMyGroupOutput { Data = result };
    }
}

