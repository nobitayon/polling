using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetRecentParticipatedPoll;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetRecentParticipatedPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetRecentParticipatedPollQuery : GetRecentParticipatedPollRequest, IRequest<GetRecentParticipatedPollOutput>
{

}

public class GetRecentParticipatedPollQueryValidator : AbstractValidator<GetRecentParticipatedPollQuery>
{
    public GetRecentParticipatedPollQueryValidator()
    {
        Include(new GetRecentParticipatedPollRequestValidator());
    }
}

public class GetRecentParticipatedPollQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetRecentParticipatedPollQuery, GetRecentParticipatedPollOutput>
{
    public async Task<GetRecentParticipatedPollOutput> Handle(GetRecentParticipatedPollQuery request, CancellationToken cancellationToken)
    {

        var groupIamIn = await databaseService.GroupMembers
                           .Where(gm => gm.Username == currentUserService.Username)
                           .Select(gm => gm.GroupId)
                           .ToListAsync(cancellationToken);

        var listPoll = await databaseService.Polls
                            .Include(p => p.Group)
                            .Where(poll => groupIamIn.Contains(poll.GroupId) && (
                                poll.Status == PollStatus.Finished ||
                                poll.Status == PollStatus.Ongoing ||
                                poll.CreatedBy == currentUserService.Username))
                            .OrderByDescending(poll => poll.Modified)
                            .OrderByDescending(poll => poll.Created)
                            .Take(5)
                            .Select(poll => new PollItem
                            {
                                Id = poll.Id,
                                GroupName = poll.Group.Name,
                                Title = poll.Title,
                                Question = poll.Question,
                                Status = poll.Status,
                                CreatedBy = poll.CreatedBy,
                                Created = poll.Created,
                                Modified = poll.Modified,
                                ModifiedBy = poll.ModifiedBy,
                            })
                            .ToListAsync(cancellationToken);

        for (var i = 0; i < listPoll.Count; i++)
        {
            var poll = listPoll[i];
            if (poll.Status == PollStatus.Finished)
            {
                var voter = await databaseService.Voters
                                .Where(v => v.PollId == poll.Id && v.Username == currentUserService.Username)
                                .SingleOrDefaultAsync(cancellationToken);

                if (voter != null)
                {
                    List<AnswerItem> answerItems = [];

                    answerItems = await databaseService.Answers
                                        .Include(a => a.Voter)
                                        .Where(a => a.Voter.PollId == poll.Id && a.Voter.Username == currentUserService.Username)
                                        .Select(a => new AnswerItem
                                        {
                                            ChoiceId = a.ChoiceId,
                                            Description = a.Choice.Description
                                        })
                                        .ToListAsync(cancellationToken);
                    listPoll[i].AnswerItems = answerItems;
                }

                var winnerAnswers = new List<ChoiceItem>();
                var listChoice = await databaseService.Choices
                                    .Where(c => c.PollId == poll.Id)
                                    .Include(c => c.Answers)
                                    .Select(c => new ChoiceItem
                                    {
                                        ChoiceId = c.Id,
                                        Description = c.Description,
                                        NumVote = c.Answers.Count
                                    })
                                    .ToListAsync();

                var maxVote = listChoice.Max(c => c.NumVote);

                foreach (var choice in listChoice)
                {
                    if (choice.NumVote == maxVote)
                    {
                        winnerAnswers.Add(new ChoiceItem { ChoiceId = choice.ChoiceId, Description = choice.Description, NumVote = choice.NumVote });
                    }
                }

                listPoll[i].WinnerAnswers = winnerAnswers;
            }
        }

        return new GetRecentParticipatedPollOutput { Data = listPoll };
    }
}
