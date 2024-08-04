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
        var queryMemberGroup = databaseService.GroupMembers
           .AsNoTracking()
           .Where(gm => gm.GroupId == request.GroupId);

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
                Username = gm.Username
            })
            .ToListAsync(cancellationToken);

        var group = await databaseService.Groups
                        .Where(g => g.Id == request.GroupId)
                        .Select(group => new GroupItem
                        {
                            Id = group.Id,
                            Name = group.Name
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

