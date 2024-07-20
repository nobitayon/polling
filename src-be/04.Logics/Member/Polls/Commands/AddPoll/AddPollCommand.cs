using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.AddPoll;
using Delta.Polling.Domain.Polls.Entities;

namespace Delta.Polling.Logics.Member.Polls.Commands.AddPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public class AddPollCommand : AddPollRequest, IRequest<AddPollOutput>
{
}

public class AddPollCommandValidator : AbstractValidator<AddPollCommand>
{
    public AddPollCommandValidator()
    {
        Include(new AddPollRequestValidator());
    }
}

public class AddPollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddPollCommand, AddPollOutput>
{
    public async Task<AddPollOutput> Handle(AddPollCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var memberGroup = await databaseService.GroupMembers
                        .Where(gm => gm.GroupId == request.GroupId)
                        .Select(gm => gm.Username)
                        .ToListAsync(cancellationToken);

        var isInGroup = memberGroup
            .Any(member =>
            {
                return member == currentUserService.Username;
            });

        if (!isInGroup)
        {
            throw new ForbiddenException($"Anda tidak bisa membuat poll di grup dengan id: {request.GroupId}, karena bukan member dari grup");
        }

        var poll = new Poll
        {
            GroupId = request.GroupId,
            Title = request.Title,
            Question = request.Question,
            MaximumAnswer = request.MaximumAnswer,
            AllowOtherChoice = request.AllowOtherChoice,
            Status = PollStatus.Draft,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username
        };

        _ = await databaseService.Polls.AddAsync(poll, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddPollOutput
        {
            Data = new AddPollResult
            {
                PollId = poll.Id
            }
        };
    }
}
