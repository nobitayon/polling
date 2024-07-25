using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Choices.Commands.AddChoice;
using Delta.Polling.Domain.Choices.Entities;

namespace Delta.Polling.Logics.Member.Choices.Commands.AddChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddChoiceCommand : AddChoiceRequest, IRequest<AddChoiceOutput>
{
}

public class AddChoiceCommandValidator : AbstractValidator<AddChoiceCommand>
{
    public AddChoiceCommandValidator()
    {
        Include(new AddChoiceRequestValidator());
    }
}

public class AddChoiceCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddChoiceCommand, AddChoiceOutput>
{
    public async Task<AddChoiceOutput> Handle(AddChoiceCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenException($"You can't add choice to this poll in this group, because you are not member of this group");
        }

        if (poll.Status != PollStatus.Draft)
        {
            throw new ForbiddenException($"Can't add choice to poll with status that is not draft");
        }

        if (poll.CreatedBy != currentUserService.Username)
        {
            throw new ForbiddenException("Can't add choice to this draft poll because this poll is not yours");
        }

        var choice = new Choice
        {
            PollId = request.PollId,
            Description = request.Description,
            IsOther = false,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username
        };

        _ = await databaseService.Choices.AddAsync(choice, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddChoiceOutput
        {
            Data = new AddChoiceResult
            {
                ChoiceId = choice.Id
            }
        };
    }
}
