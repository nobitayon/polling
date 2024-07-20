using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.Logics.Member.Polls.Queries.GetPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public class GetPollQuery : GetPollRequest, IRequest<GetPollOutput>
{

}

public class GetPollQueryValidator : AbstractValidator<GetPollQuery>
{
    public GetPollQueryValidator()
    {
        Include(new GetPollRequestValidator());
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
                        .Where(poll => poll.Id == request.PollId)
                        .Select(poll => new
                        {
                            GroupId = poll.GroupId,
                            Status = poll.Status,
                            Title = poll.Title,
                            AllowOtherChoice = poll.AllowOtherChoice,
                            MaximumAnswer = poll.MaximumAnswer,
                            Question = poll.Question,
                            CreatedBy = poll.CreatedBy
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
            throw new ForbiddenException($"You can't access this poll, because you are not member of group");
        }

        if (!(pollDetails.CreatedBy == currentUserService.Username || pollDetails.Status is PollStatus.Finished || pollDetails.Status is PollStatus.Ongoing))
        {
            throw new ForbiddenException($"You can't access this poll, because this poll is not published yet");
        }

        var choiceItems = await databaseService.Choices
                            .Where(c => c.PollId == request.PollId)
                            .Select(c => new ChoiceItem
                            {
                                Description = c.Description,
                                IsOther = c.IsOther
                            }).ToListAsync(cancellationToken);

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
                                    ChoiceId = a.ChoiceId
                                }).ToListAsync(cancellationToken);
        }

        var pollItem = new PollItem
        {
            Status = pollDetails.Status,
            Title = pollDetails.Title,
            Question = pollDetails.Question,
            MaximumAnswer = pollDetails.MaximumAnswer,
            AllowOtherChoice = pollDetails.AllowOtherChoice,
            AnswerItems = answerItems,
            ChoiceItems = choiceItems
        };

        return new GetPollOutput { Data = pollItem };
    }
}
