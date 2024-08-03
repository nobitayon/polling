using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Logics.Member.Groups.Queries.GetMyGroup;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyGroupQuery : GetMyGroupRequest, IRequest<GetMyGroupOutput>
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

        var query = databaseService.Polls
            .Include(p => p.Group)
            .AsNoTracking()
            .Where(p => p.GroupId == request.GroupId && (p.Status == PollStatus.Ongoing || p.Status == PollStatus.Finished));

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.OrderBy(poll => poll.Title);
        }
        else
        {
            var sortOrder = request.SortOrder is not null
                ? request.SortOrder.Value
                : SortOrder.Asc;

            if (sortOrder is SortOrder.Asc)
            {
                if (request.SortField == nameof(Poll.Title))
                {
                    query = query.OrderBy(poll => poll.Title);
                }
                else if (request.SortField == nameof(Poll.Created))
                {
                    query = query.OrderBy(poll => poll.Created);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(Poll.Title))
                {
                    query = query.OrderByDescending(poll => poll.Title);
                }
                else if (request.SortField == nameof(Poll.Created))
                {
                    query = query.OrderByDescending(poll => poll.Created);
                }
            }
            else
            {
                query = query.OrderBy(poll => poll.Created);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchField) && !string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (request.SearchField == nameof(Poll.Title))
            {
                query = query.Where(poll => poll.Title.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Question))
            {
                query = query.Where(poll => poll.Question.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Group.Name))
            {
                query = query.Where(poll => poll.Group.Name.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Status))
            {
                if (request.SearchText == nameof(PollStatus.Draft))
                {
                    query = query.Where(poll => poll.Status == PollStatus.Draft);
                }
                else if (request.SearchText == nameof(PollStatus.Ongoing))
                {
                    query = query.Where(poll => poll.Status == PollStatus.Ongoing);
                }
                else if (request.SearchText == nameof(PollStatus.Finished))
                {
                    query = query.Where(poll => poll.Status == PollStatus.Finished);
                }
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var polls = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(poll => new PollItem
            {
                Id = poll.Id,
                Title = poll.Title,
                Status = poll.Status,
                Created = poll.Created,
                CreatedBy = poll.CreatedBy,
                GroupName = poll.Group.Name
            })
            .ToListAsync(cancellationToken);

        var pollItems = new PaginatedListResponse<PollItem>
        {
            Items = polls,
            TotalCount = totalCount
        };

        var output = new GetMyGroupOutput
        {
            Data = new GroupItem
            {
                Id = groupDetails.Id,
                Name = groupDetails.Name,
                Created = groupDetails.Created,
                CreatedBy = groupDetails.CreatedBy,
                MemberItems = memberGroup,
                PollItems = pollItems
            }
        };

        return output;
    }
}

