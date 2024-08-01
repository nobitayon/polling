using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.FinishPoll;
using Delta.Polling.Services.Email;
using Delta.Polling.Services.UserProfile;

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

public record UsernameWithEmail
{
    public required string Username { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}

public class FinishPollCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IEmailService emailService,
    IUserProfileService userProfileService)
    : IRequestHandler<FinishPollCommand>
{
    public async Task Handle(FinishPollCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var poll = await databaseService.Polls
                    .Include(p => p.Group)
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
        poll.Modified = DateTimeOffset.Now;
        poll.ModifiedBy = currentUserService.Username;

        _ = await databaseService.SaveAsync(cancellationToken);

        var listRecipientUsername = await databaseService.Voters
                                    .Where(v => v.PollId == request.PollId)
                                    .Select(v => v.Username)
                                    .ToListAsync(cancellationToken);

        var listRecipientWithEmail = new List<UsernameWithEmail>();
        foreach (var recipientUsername in listRecipientUsername)
        {
            var responseGetEmail = userProfileService.GetUserProfileAsync(recipientUsername, cancellationToken)
                ?? throw new Exception("Error get email");

            if (responseGetEmail.Result is null)
            {
                throw new Exception("Error get email");
            }

            listRecipientWithEmail.Add(new UsernameWithEmail
            {
                Email = responseGetEmail.Result.Email,
                Name = recipientUsername,
                Username = responseGetEmail.Result.Name
            });
        }

        var tos = listRecipientWithEmail.Select(recipient => new MailBoxModel
        {
            Name = recipient.Name,
            Address = recipient.Email
        });

        var input = new SendEmailInput
        {
            Tos = tos,
            Ccs = [],
            Subject = $"Poll with title {poll.Title} in group {poll.Group.Name} already finished",
            Body = $"You can go to /poll/details/{request.PollId} to see result"
        };

        emailService.SendEmail(input);
    }
}
