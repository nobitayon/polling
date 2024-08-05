using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.StartPoll;

namespace Delta.Polling.Logics.Member.Polls.Commands.StartPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record StartPollCommand : StartPollRequest, IRequest
{
}

public class StartPollCommandValidator : AbstractValidator<StartPollCommand>
{
    public StartPollCommandValidator()
    {
        Include(new StartPollRequestValidator());
    }
}

public class StartPollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<StartPollCommand>
{
    public async Task Handle(StartPollCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var poll = await databaseService.Polls
                    .Where(p => p.Id == request.PollId)
                    .SingleOrDefaultAsync(cancellationToken)
                    ?? throw new EntityNotFoundException("Poll", request.PollId);

        if (poll.Status is not PollStatus.Draft)
        {
            throw new ForbiddenException($"Can't start poll with status that is not draft");
        }

        var memberGroup = await databaseService.GroupMembers
                        .Where(gm => gm.GroupId == poll.GroupId)
                        .Select(gm => gm.Username)
                        .ToListAsync(cancellationToken);

        var isInGroup = memberGroup
            .Any(member =>
            {
                return member == currentUserService.Username;
            });

        if (!isInGroup)
        {
            throw new ForbiddenException($"Can't start poll: {request.PollId}, because you are not member of this group");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"Can't start poll: {request.PollId}, because this is not your poll");
        }

        var numChoiceExist = await databaseService.Choices
                                .Where(c => c.PollId == request.PollId)
                                .ToListAsync(cancellationToken);

        if (numChoiceExist.Count() <= 1)
        {
            throw new ForbiddenException($"Can't start poll: {request.PollId}, because number of choice is less or equal 1");
        }

        poll.Status = PollStatus.Ongoing;
        poll.Modified = DateTimeOffset.Now;
        poll.ModifiedBy = currentUserService.Username;

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
