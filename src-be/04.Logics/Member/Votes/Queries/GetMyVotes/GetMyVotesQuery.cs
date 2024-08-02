using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Logics.Member.Votes.Queries.GetMyVotes;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyVotesQuery : GetMyVotesRequest, IRequest<GetMyVotesOutput>
{

}

public class GetMyVotesQueryValidator : AbstractValidator<GetMyVotesQuery>
{
    public GetMyVotesQueryValidator()
    {
        Include(new GetMyVotesRequestValidator());
    }
}

public class GetMyVotesQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyVotesQuery, GetMyVotesOutput>
{
    public async Task<GetMyVotesOutput> Handle(GetMyVotesQuery request, CancellationToken cancellationToken)
    {
        var query = databaseService.Voters
            .Where(v => v.Username == currentUserService.Username)
            .Include(v => v.Poll)
                .ThenInclude(p => p.Group)
            .Include(v => v.Poll)
                .ThenInclude(p => p.Choices)
                    .ThenInclude(c => c.Answers)
            .Include(v => v.Answers)
                .ThenInclude(a => a.Choice)
            .AsNoTracking();

        if (string.IsNullOrWhiteSpace(request.SortField))
        {
            query = query.OrderBy(v => v.Poll.Title);
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
                    query = query.OrderBy(v => v.Poll.Title);
                }
                else if (request.SortField == nameof(Poll.Created))
                {
                    query = query.OrderBy(v => v.Poll.Created);
                }
            }
            else if (sortOrder is SortOrder.Desc)
            {
                if (request.SortField == nameof(Poll.Title))
                {
                    query = query.OrderByDescending(v => v.Poll.Title);
                }
                else if (request.SortField == nameof(Poll.Created))
                {
                    query = query.OrderByDescending(v => v.Poll.Created);
                }
            }
            else
            {
                query = query.OrderBy(v => v.Poll.Title);
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchField) && !string.IsNullOrWhiteSpace(request.SearchText))
        {
            if (request.SearchField == nameof(Poll.Title))
            {
                query = query.Where(v => v.Poll.Title.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Question))
            {
                query = query.Where(v => v.Poll.Question.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(VoteItem.GroupName))
            {
                query = query.Where(v => v.Poll.Group.Name.ToLower().Contains(request.SearchText!.ToLower()));
            }
            else if (request.SearchField == nameof(Poll.Status))
            {
                if (request.SearchText == nameof(PollStatus.Draft))
                {
                    query = query.Where(v => v.Poll.Status == PollStatus.Draft);
                }
                else if (request.SearchText == nameof(PollStatus.Ongoing))
                {
                    query = query.Where(v => v.Poll.Status == PollStatus.Ongoing);
                }
                else if (request.SearchText == nameof(PollStatus.Finished))
                {
                    query = query.Where(v => v.Poll.Status == PollStatus.Finished);
                }
            }
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var skippedAmount = PagerHelper.GetSkipAmount(request.Page, request.PageSize);

        var votes = await query
            .Skip(skippedAmount)
            .Take(request.PageSize)
            .Select(v => new VoteItem
            {
                Id = v.Id,
                PollId = v.PollId,
                GroupName = v.Poll.Group.Name,
                Status = v.Poll.Status,
                PollQuestion = v.Poll.Question,
                PollTitle = v.Poll.Title,
                Created = v.Poll.Created,
                ChoiceItems = v.Poll.Choices.Select(
                        c => new ChoiceItem
                        {
                            Id = c.Id,
                            Description = c.Description,
                            NumVote = c.Answers.Count
                        }
                ),
                AnswerItems = v.Answers.Select(
                        a => new AnswerItem
                        {
                            Id = a.ChoiceId,
                            Description = a.Choice.Description
                        }
                    )

            })
            .ToListAsync(cancellationToken);

        var output = new GetMyVotesOutput
        {
            Data = new PaginatedListResponse<VoteItem>
            {
                Items = votes,
                TotalCount = totalCount
            }
        };

        return output;
    }
}
