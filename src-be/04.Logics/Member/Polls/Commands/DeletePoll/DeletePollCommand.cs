using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.DeletePoll;

namespace Delta.Polling.Logics.Member.Polls.Commands.DeletePoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record DeletePollCommand : DeletePollRequest, IRequest
{
}

public class DeletePollCommandValidator : AbstractValidator<DeletePollCommand>
{
    public DeletePollCommandValidator()
    {
        Include(new DeletePollRequestValidator());
    }
}

public class DeletePollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeletePollCommand>
{
    public async Task Handle(DeletePollCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenException($"Can't delete poll with status that is not draft");
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
            throw new Exception($"Can't delete poll: {request.PollId}, because you are not member of this group");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new Exception($"Can't delete poll: {request.PollId}, because this is not your poll");
        }

        _ = databaseService.Polls.Remove(poll);

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
