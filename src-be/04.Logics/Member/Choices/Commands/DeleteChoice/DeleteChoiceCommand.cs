using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Choices.Commands.DeleteChoice;

namespace Delta.Polling.Logics.Member.Choices.Commands.DeleteChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record DeleteChoiceCommand : DeleteChoiceRequest, IRequest
{
}

public class DeleteChoiceCommandValidator : AbstractValidator<DeleteChoiceCommand>
{
    public DeleteChoiceCommandValidator()
    {
        Include(new DeleteChoiceRequestValidator());
    }
}

public class DeleteChoiceCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<DeleteChoiceCommand>
{
    public async Task Handle(DeleteChoiceCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var choice = await databaseService.Choices
                       .Where(c => c.Id == request.ChoiceId)
                       .SingleOrDefaultAsync(cancellationToken)
                       ?? throw new Exception($"Choice with Id: {request.ChoiceId} not exist");

        var poll = await databaseService.Polls
                        .Where(p => p.Id == choice.PollId)
                        .SingleOrDefaultAsync(cancellationToken)
                        ?? throw new Exception($"Poll with Id: {choice.PollId} not exist");

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
            throw new Exception($"Can't delete choice to this poll in this group, because you are not member of this group");
        }

        if (poll.Status != PollStatus.Draft)
        {
            throw new Exception($"Can't delete choice to poll with status that is not draft");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new Exception("Can't delete choice to this draft poll because this poll is not yours");
        }

        _ = databaseService.Choices.Remove(choice);

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}
