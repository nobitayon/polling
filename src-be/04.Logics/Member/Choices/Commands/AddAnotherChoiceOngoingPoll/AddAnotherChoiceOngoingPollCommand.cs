using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.Domain.Choices.Entities;

namespace Delta.Polling.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddAnotherChoiceOngoingPollCommand : AddAnotherChoiceOngoingPollRequest, IRequest<AddAnotherChoiceOngoingPollOutput>
{
}

public class AddAnotherChoiceOngoingPollCommandValidator : AbstractValidator<AddAnotherChoiceOngoingPollCommand>
{
    public AddAnotherChoiceOngoingPollCommandValidator()
    {
        Include(new AddAnotherChoiceOngoingPollRequestValidator());
    }
}

public class AddAnotherChoiceOngoingPollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddAnotherChoiceOngoingPollCommand, AddAnotherChoiceOngoingPollOutput>
{
    public async Task<AddAnotherChoiceOngoingPollOutput> Handle(AddAnotherChoiceOngoingPollCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var poll = await databaseService.Polls
                        .Where(p => p.Id == request.PollId)
                        .SingleOrDefaultAsync(cancellationToken)
                        ?? throw new EntityNotFoundException("Poll", request.PollId);

        if (poll.Status != PollStatus.Ongoing)
        {
            throw new Exception($"Can't add another choice to poll with status that is not ongoing");
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
            throw new Exception($"You can't add another choice to this poll in this group, because you are not member of this group");
        }

        var choiceAddedByThisUser = await databaseService.Choices
                                        .Where(c => c.CreatedBy == currentUserService.Username && c.PollId == request.PollId && c.IsOther)
                                        .ToListAsync(cancellationToken);

        // TODO: Asumsikan semua orang hanya boleh add choice-nya 1 maks
        if (choiceAddedByThisUser.Count() >= 1)
        {
            throw new Exception($"You already add another choice to this ongoing poll");
        }

        var choice = new Choice
        {
            PollId = request.PollId,
            Description = request.Description,
            IsOther = true,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username
        };

        _ = await databaseService.Choices.AddAsync(choice, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddAnotherChoiceOngoingPollOutput
        {
            Data = new AddAnotherChoiceOngoingPollResult
            {
                ChoiceId = choice.Id
            }
        };
    }
}
