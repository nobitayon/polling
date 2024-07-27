using Delta.Polling.Both.Member.Polls.Queries.GetPollWithAllAnswer;
using Delta.Polling.Base.Polls.Enums;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetPollWithAllAnswer;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetPollWithAllAnswerQuery : GetPollWithAllAnswerRequest, IRequest<GetPollWithAllAnswerOutput>
{

}

public class GetPollWithAllAnswerQueryValidator : AbstractValidator<GetPollWithAllAnswerQuery>
{
    public GetPollWithAllAnswerQueryValidator()
    {
        Include(new GetPollWithAllAnswerRequestValidator());
    }
}

public class GetPollWithAllAnswerQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetPollWithAllAnswerQuery, GetPollWithAllAnswerOutput>
{
    public async Task<GetPollWithAllAnswerOutput> Handle(GetPollWithAllAnswerQuery request, CancellationToken cancellationToken)
    {

        var pollDetails = await databaseService.Polls
                        .Where(poll => poll.Id == request.PollId)
                        .Select(poll => new
                        {
                            Id = poll.Id,
                            GroupId = poll.GroupId,
                            Status = poll.Status,
                            Title = poll.Title,
                            Question = poll.Question,
                            CreatedBy = poll.CreatedBy,
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
            throw new Exception($"You can't access this poll, because you are not member of group");
        }

        if (!(pollDetails.CreatedBy == currentUserService.Username || pollDetails.Status is PollStatus.Finished || pollDetails.Status is PollStatus.Ongoing))
        {
            throw new Exception($"You can't access this poll, because this poll is not published yet");
        }

        List<AnswerItem> answerItems = [];

        answerItems = await databaseService.Answers
                            .Include(a => a.Voter)
                            .Where(a => a.Voter.PollId == request.PollId)
                            .GroupBy(a => a.ChoiceId)
                            .Select(g => new AnswerItem
                            {
                                ChoiceId = g.Key,
                                Description = g.First().Choice.Description,
                                Count = g.Count()
                            })
                            .ToListAsync(cancellationToken);

        var choiceItems = await databaseService.Choices
                            .Include(c => c.Answers)
                            .Where(c => c.PollId == request.PollId)
                            .Select(c => new ChoiceItem
                            {
                                ChoiceId = c.Id,
                                Description = c.Description,
                                Count = c.Answers.Count
                            }).ToListAsync(cancellationToken);

        var pollItem = new PollItem
        {
            Id = pollDetails.Id,
            Title = pollDetails.Title,
            Question = pollDetails.Question,
            AnswerItems = answerItems,
            ChoiceItems = choiceItems
        };

        return new GetPollWithAllAnswerOutput { Data = pollItem };
    }
}
