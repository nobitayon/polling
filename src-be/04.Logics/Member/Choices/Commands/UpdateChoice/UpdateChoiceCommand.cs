using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Choices.Commands.UpdateChoice;

namespace Delta.Polling.Logics.Member.Choices.Commands.UpdateChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record UpdateChoiceCommand : UpdateChoiceRequest, IRequest
{
}

public class UpdateChoiceCommandValidator : AbstractValidator<UpdateChoiceCommand>
{
    public UpdateChoiceCommandValidator()
    {
        Include(new UpdateChoiceRequestValidator());
    }
}

public class UpdateChoiceCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateChoiceCommand>
{
    public async Task Handle(UpdateChoiceCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var poll = await databaseService.Polls
                        .Where(p => p.Id == request.PollId)
                        .SingleOrDefaultAsync(cancellationToken)
                        ?? throw new Exception($"Poll with Id: {request.PollId} not exist");

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
            throw new ForbiddenException($"You can't update choice to this poll in this group, because you are not member of this group");
        }

        if (poll.Status != PollStatus.Draft)
        {
            throw new ForbiddenException($"Can't update choice to poll with status that is not draft");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException("Can't update choice to this draft poll because this poll is not yours");
        }

        var choice = await databaseService.Choices
                       .Where(c => c.Id == request.ChoiceId)
                       .SingleOrDefaultAsync(cancellationToken)
                       ?? throw new Exception($"Choice with Id: {request.PollId} not exist");

        choice.Description = request.Description;
        choice.Modified = DateTimeOffset.Now;
        choice.ModifiedBy = currentUserService.Username;

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
