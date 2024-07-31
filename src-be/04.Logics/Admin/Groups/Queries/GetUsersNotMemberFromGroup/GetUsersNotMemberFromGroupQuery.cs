using Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;
using Delta.Polling.Services.UserProfile;

namespace Delta.Polling.Logics.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

[Authorize(RoleName = RoleNameFor.Admin)]
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
    IDatabaseService databaseService,
    IUserProfileService userProfileService)
    : IRequestHandler<GetUsersNotMemberFromGroupQuery, GetUsersNotMemberFromGroupOutput>
{
    public async Task<GetUsersNotMemberFromGroupOutput> Handle(GetUsersNotMemberFromGroupQuery request, CancellationToken cancellationToken)
    {
        List<string> listMember = [];
        var response = userProfileService.GetUsersAsync(cancellationToken);

        if (response != null)
        {
            foreach (var check in response.Result)
            {
                listMember.Add(check.Username);
            }
        }
        else
        {
            throw new Exception("Error getting users from simpletor");
        }

        List<string> notAMember = [];

        foreach (var username in listMember)
        {
            var groupMember = await databaseService.GroupMembers
                                .Where(gm => gm.Username == username && gm.GroupId == request.GroupId)
                                .SingleOrDefaultAsync(cancellationToken);

            if (groupMember is null)
            {
                notAMember.Add(username);
            }
        }

        var totalCount = notAMember.Count;

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            notAMember = [.. notAMember.OrderBy(s => s)];
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
                    notAMember = [.. notAMember.OrderBy(s => s)];
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(MemberItem.Username))
                {
                    notAMember = [.. notAMember.OrderByDescending(s => s)];
                }
            }
            else
            {
                notAMember = [.. notAMember.OrderBy(s => s)];
            }
        }

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var members = notAMember
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(member => new MemberItem { Username = member })
            .ToList();

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
