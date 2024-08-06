using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetPollQuery : GetPollRequest, IRequest<GetPollOutput>
{

}

public class GetPollQueryValidator : AbstractValidator<GetPollQuery>
{
    public GetPollQueryValidator()
    {
        Include(new GetPollRequestValidator());
    }
}

public static class Helper
{
    public static bool IsItChosen(Guid choiceId, List<AnswerItem> answerItems)
    {
        if (answerItems.Count == 0)
        {
            return false;
        }

        return answerItems.Any(a => a.ChoiceId == choiceId);
    }
}

public class GetPollQueryHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetPollQuery, GetPollOutput>
{
    public async Task<GetPollOutput> Handle(GetPollQuery request, CancellationToken cancellationToken)
    {

        var pollDetails = await databaseService.Polls
                        .Include(p => p.Group)
                        .Where(poll => poll.Id == request.PollId)
                        .Select(poll => new
                        {
                            Id = poll.Id,
                            GroupId = poll.GroupId,
                            GroupName = poll.Group.Name,
                            Status = poll.Status,
                            Title = poll.Title,
                            AllowOtherChoice = poll.AllowOtherChoice,
                            MaximumAnswer = poll.MaximumAnswer,
                            Question = poll.Question,
                            Created = poll.Created,
                            CreatedBy = poll.CreatedBy,
                            Modified = poll.Modified,
                            ModifiedBy = poll.ModifiedBy
                        }).SingleOrDefaultAsync(cancellationToken)
                        ?? throw new EntityNotFoundException("Poll", request.PollId);

        if (pollDetails.Status == PollStatus.Abandoned)
        {
            throw new Exception("You can't access abandoned poll");
        }

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

        var voterId = await databaseService.Voters
                        .Where(v => v.PollId == request.PollId && v.Username == currentUserService.Username)
                        .Select(v => v.Id)
                        .SingleOrDefaultAsync(cancellationToken);

        List<AnswerItem> answerItems = [];

        if (voterId != Guid.Empty)
        {
            answerItems = await databaseService.Answers
                                .Include(a => a.Choice)
                                .Where(a => a.VoterId == voterId)
                                .Select(a => new AnswerItem
                                {
                                    ChoiceId = a.ChoiceId,
                                    Description = a.Choice.Description
                                }).ToListAsync(cancellationToken);
        }

        var choiceItems = await databaseService.Choices
                            .Include(c => c.Answers)
                            .Where(c => c.PollId == request.PollId)
                            .Select(c => new ChoiceItem
                            {
                                Id = c.Id,
                                Description = c.Description,
                                IsOther = c.IsOther,
                                IsChosen = Helper.IsItChosen(c.Id, answerItems),
                                NumVote = c.Answers.Count,
                                CreatedBy = c.CreatedBy,
                                IsDisabled = c.IsOther && c.CreatedBy == currentUserService.Username
                            }).ToListAsync(cancellationToken);

        var pollItem = new PollItem
        {
            Id = pollDetails.Id,
            GroupName = pollDetails.GroupName,
            Status = pollDetails.Status,
            Title = pollDetails.Title,
            Question = pollDetails.Question,
            MaximumAnswer = pollDetails.MaximumAnswer,
            AllowOtherChoice = pollDetails.AllowOtherChoice,
            Created = pollDetails.Created,
            CreatedBy = pollDetails.CreatedBy,
            Modified = pollDetails.Modified,
            ModifiedBy = pollDetails.ModifiedBy,
            AnswerItems = answerItems,
            ChoiceItems = choiceItems
        };

        return new GetPollOutput { Data = pollItem };
    }
}
