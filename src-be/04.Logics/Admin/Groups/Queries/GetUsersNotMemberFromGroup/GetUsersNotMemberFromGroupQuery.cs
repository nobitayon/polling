using Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;
using Delta.Polling.Domain.Groups.Entities;
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
        var listOfFullProfile = await userProfileService.GetUsersAsync(cancellationToken) ?? throw new Exception("Error getting users from simpletor");

        foreach (var profileItem in listOfFullProfile)
        {
            listMember.Add(profileItem.Username);
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

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            notAMember.Sort(StringComparer.OrdinalIgnoreCase);

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
                    notAMember.Sort(StringComparer.OrdinalIgnoreCase);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(MemberItem.Username))
                {
                    notAMember.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(y, x));
                }
            }
            else
            {
                notAMember.Sort(StringComparer.OrdinalIgnoreCase);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchField) && !string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (request.SearchField == nameof(GroupMember.Username))
            {
                notAMember = [.. notAMember.Where(s => s.ToLower().Contains(request.SearchText!.ToLower()))];
            }
        }

        var totalCount = notAMember.Count;

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var members = notAMember
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(member => new MemberItem { Username = member })
            .ToList();

        var group = await databaseService.Groups
                        .Where(g => g.Id == request.GroupId)
                        .Select(group => new GroupItem
                        {
                            Id = group.Id,
                            Name = group.Name
                        }).SingleOrDefaultAsync(cancellationToken)
                        ?? throw new EntityNotFoundException(nameof(Group), request.GroupId);

        var data = new GetUsersNotMemberFromGroupResult
        {
            GroupItem = group,
            MemberItems = new PaginatedListResponse<MemberItem>
            {
                Items = members,
                TotalCount = totalCount
            }
        };
        Console.WriteLine("berhasil 123");
        return new GetUsersNotMemberFromGroupOutput { Data = data };
    }
}
