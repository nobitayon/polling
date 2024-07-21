using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.UpdatePoll;

namespace Delta.Polling.Logics.Member.Polls.Commands.UpdatePoll;

[Authorize(RoleName = RoleNameFor.Member)]
public class UpdatePollCommand : UpdatePollRequest, IRequest
{
}

public class UpdatePollCommandValidator : AbstractValidator<UpdatePollCommand>
{
    public UpdatePollCommandValidator()
    {
        Include(new UpdatePollRequestValidator());
    }
}

public class UpdatePollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdatePollCommand>
{
    public async Task Handle(UpdatePollCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenException($"Can't update poll with status that is not draft");
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
            throw new ForbiddenException($"Can't update poll: {request.PollId}, because you are not member of this group");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException($"Can't update poll: {request.PollId}, because this is not your poll");
        }

        poll.Modified = DateTimeOffset.Now;
        poll.ModifiedBy = currentUserService.Username;
        poll.AllowOtherChoice = request.AllowOtherChoice;
        poll.MaximumAnswer = request.MaximumAnswer;
        poll.AllowOtherChoice = request.AllowOtherChoice;
        poll.Title = request.Title;
        poll.Question = request.Question;

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
