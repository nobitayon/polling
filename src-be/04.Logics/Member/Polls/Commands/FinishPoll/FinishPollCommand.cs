using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.FinishPoll;

namespace Delta.Polling.Logics.Member.Polls.Commands.FinishPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record FinishPollCommand : FinishPollRequest, IRequest
{
}

public class FinishPollCommandValidator : AbstractValidator<FinishPollCommand>
{
    public FinishPollCommandValidator()
    {
        Include(new FinishPollRequestValidator());
    }
}

public class FinishPollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<FinishPollCommand>
{
    public async Task Handle(FinishPollCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var poll = await databaseService.Polls
                    .Where(p => p.Id == request.PollId)
                    .SingleOrDefaultAsync(cancellationToken)
                    ?? throw new EntityNotFoundException("Poll", request.PollId);

        if (poll.Status is not PollStatus.Ongoing)
        {
            throw new ForbiddenException($"Can't finish poll with status that is not ongoing");
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
            throw new ForbiddenException($"Can't finish poll: {request.PollId}, because you are not member of this group");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"Can't finish poll: {request.PollId}, because this is not your poll");
        }

        poll.Status = PollStatus.Finished;

        _ = await databaseService.SaveAsync(cancellationToken);

        // TODO: Send email to all voter
    }
}
