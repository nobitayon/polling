using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Choices.Queries.GetChoicesByPoll;

namespace Delta.Polling.Logics.Member.Choices.Queries.GetChoicesByPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetChoicesByPollQuery : GetChoicesByPollRequest, IRequest<GetChoicesByPollOutput>
{

}

public class GetChoicesByPollQueryValidator : AbstractValidator<GetChoicesByPollQuery>
{
    public GetChoicesByPollQueryValidator()
    {
        Include(new GetChoicesByPollRequestValidator());
    }
}

public class GetChoicesByPollQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetChoicesByPollQuery, GetChoicesByPollOutput>
{
    public async Task<GetChoicesByPollOutput> Handle(GetChoicesByPollQuery request, CancellationToken cancellationToken)
    {
        // Sekarang dijadiin untuk dapetin choice di live result
        var pollDetails = await databaseService.Polls
                        .Where(poll => poll.Id == request.PollId)
                        .Select(poll => new
                        {
                            poll.Id,
                            poll.GroupId,
                            poll.CreatedBy,
                            poll.Status
                        }).SingleOrDefaultAsync(cancellationToken)
                        ?? throw new EntityNotFoundException("Poll", request.PollId);

        var memberGroup = await databaseService.GroupMembers
            .Where(gm => gm.GroupId == pollDetails.GroupId)
            .Select(gm => gm.Username)
            .ToListAsync(cancellationToken);

        var isInGroup = memberGroup
           .Any(member =>
           {
               return member == currentUserService.Username;
           });

        if (!isInGroup)
        {
            throw new Exception($"You can't access choices of this poll, because you are not member of group");
        }

        if (!(pollDetails.CreatedBy == currentUserService.Username && pollDetails.Status is PollStatus.Ongoing))
        {
            throw new Exception($"You can't see live result of this poll");
        }

        var choiceItems = await databaseService.Choices
                            .Include(c => c.Answers)
                            .Where(c => c.PollId == request.PollId)
                            .Select(c => new ChoiceItem
                            {
                                Id = c.Id,
                                Description = c.Description,
                                IsOther = c.IsOther,
                                NumVote = c.Answers.Count()
                            })
                            .OrderBy(c => c.Description)
                            .ToListAsync(cancellationToken);

        return new GetChoicesByPollOutput { Data = choiceItems };
    }
}
