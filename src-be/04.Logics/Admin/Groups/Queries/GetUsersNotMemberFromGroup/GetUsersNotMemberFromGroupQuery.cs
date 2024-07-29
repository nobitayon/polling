using Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

namespace Delta.Polling.Logics.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

[Authorize(RoleName = RoleNameFor.Administrator)]
public record GetUsersNotMemberFromGroupQuery : GetUsersNotMemberFromGroupRequest, IRequest<GetUsersNotMemberFromGroupOutput>
{

}

public class GetUsersNotMemberFromGroupQueryValidator : AbstractValidator<GetUsersNotMemberFromGroupQuery>
{
    public GetUsersNotMemberFromGroupQueryValidator()
    {
        Include(new GetUsersNotMemberFromGroupRequestValidator());
    }
}

public class GetUsersNotMemberFromGroupQueryHandler(
    IDatabaseService databaseService)
    : IRequestHandler<GetUsersNotMemberFromGroupQuery, GetUsersNotMemberFromGroupOutput>
{
    public async Task<GetUsersNotMemberFromGroupOutput> Handle(GetUsersNotMemberFromGroupQuery request, CancellationToken cancellationToken)
    {
        var query = databaseService.GroupMembers
            .Where(gm => gm.GroupId == request.GroupId)
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.OrderBy(gm => gm.Username);
        }
        else
        {
            var sortOrder = request.SortOrder is not null
                ? request.SortOrder.Value
                : SortOrder.Asc;

            if (sortOrder is SortOrder.Asc)
            {
                if (request.SortField == nameof(MemberItem.Username))
                {
                    query = query.OrderBy(gm => gm.Username);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(MemberItem.Username))
                {
                    query = query.OrderByDescending(gm => gm.Username);
                }
            }
            else
            {
                query = query.OrderBy(gm => gm.Username);
            }
        }

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var members = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(gm => new MemberItem
            {
                Username = gm.Username
            })
            .ToListAsync(cancellationToken);

        var output = new GetUsersNotMemberFromGroupOutput
        {
            Data = new PaginatedListResponse<MemberItem>
            {
                Items = members,
                TotalCount = totalCount
            }
        };

        return output;
    }
}
