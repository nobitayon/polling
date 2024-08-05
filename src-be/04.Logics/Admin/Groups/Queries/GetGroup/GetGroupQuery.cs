using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Admin.Groups.Queries.GetGroup;
using Delta.Polling.Domain.Groups.Entities;

namespace Delta.Polling.Logics.Admin.Groups.Queries.GetGroup;

[Authorize(RoleName = RoleNameFor.Admin)]
public record GetGroupQuery : GetGroupRequest, IRequest<GetGroupOutput>
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
        //var queryMemberGroup = databaseService.GroupMembers
        //   .AsNoTracking()
        //   .Where(gm => gm.GroupId == request.GroupId);

        var queryMemberGroup = databaseService.GroupMembers
                   .Where(gm => gm.GroupId == request.GroupId)
                   .GroupJoin(
                       databaseService.Polls,
                       gm => gm.Username,
                       p => p.CreatedBy,
                       (gm, p) => new
                       {
                           Id = gm.Id,
                           Username = gm.Username,
                           NumOngoingPoll = p.Count(p => p.Status == PollStatus.Ongoing && p.GroupId == gm.GroupId)
                       });

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            queryMemberGroup = queryMemberGroup.OrderBy(gm => gm.Username);
        }
        else
        {
            var sortOrder = request.SortOrder is not null
                ? request.SortOrder.Value
                : SortOrder.Asc;

            if (sortOrder is SortOrder.Asc)
            {
                if (request.SortField == nameof(GroupMember.Username))
                {
                    queryMemberGroup = queryMemberGroup.OrderBy(gm => gm.Username);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(GroupMember.Username))
                {
                    Console.WriteLine("masuk sini kan DESC");
                    queryMemberGroup = queryMemberGroup.OrderByDescending(gm => gm.Username);
                }
            }
            else
            {
                queryMemberGroup = queryMemberGroup.OrderBy(gm => gm.Username);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchField) && !string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (request.SearchField == nameof(GroupMember.Username))
            {
                queryMemberGroup = queryMemberGroup.Where(gm => gm.Username.ToLower().Contains(request.SearchText!.ToLower()));
            }
        }

        var totalCount = await queryMemberGroup.CountAsync(cancellationToken);

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var members = await queryMemberGroup
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(gm => new MemberItem
            {
                GroupMemberId = gm.Id,
                Username = gm.Username,
                NumOngoingPoll = gm.NumOngoingPoll
            })
            .ToListAsync(cancellationToken);

        //var group = await databaseService.Groups
        //                .Where(g => g.Id == request.GroupId)
        //                .Select(group => new GroupItem
        //                {
        //                    Id = group.Id,
        //                    Name = group.Name
        //                }).SingleOrDefaultAsync(cancellationToken)
        //                ?? throw new EntityNotFoundException(nameof(Group), request.GroupId);

        var group = await databaseService.Groups
                        .Where(g => g.Id == request.GroupId)
                        .GroupJoin(
                       databaseService.Polls,
                       g => g.Id,
                       p => p.GroupId,
                       (g, p) => new
                       {
                           Id = g.Id,
                           Name = g.Name,
                           NumOngoingPoll = p.Count(p => p.Status == PollStatus.Ongoing)
                       })
                        .Select(group => new GroupItem
                        {
                            Id = group.Id,
                            Name = group.Name,
                            NumOngoingPoll = group.NumOngoingPoll
                        }).SingleOrDefaultAsync(cancellationToken)
                        ?? throw new EntityNotFoundException(nameof(Group), request.GroupId);

        var data = new GetGroupResult
        {
            GroupItem = group,
            MemberItems = new PaginatedListResponse<MemberItem>
            {
                Items = members,
                TotalCount = totalCount
            }
        };

        return new GetGroupOutput { Data = data };
    }
}

