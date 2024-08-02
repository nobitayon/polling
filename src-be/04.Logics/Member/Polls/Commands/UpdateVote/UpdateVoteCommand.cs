using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.UpdateVote;
using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Logics.SignalR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Delta.Polling.Logics.Member.Polls.Commands.UpdateVote;

[Authorize(RoleName = RoleNameFor.Member)]
public record UpdateVoteCommand : UpdateVoteRequest, IRequest<UpdateVoteOutput>
{
}

public class UpdateVoteCommandValidator : AbstractValidator<UpdateVoteCommand>
{
    public UpdateVoteCommandValidator()
    {
        Include(new UpdateVoteRequestValidator());
    }
}

public class UpdateVoteCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IHubContext<PollHub> hubContext)
    : IRequestHandler<UpdateVoteCommand, UpdateVoteOutput>
{
    public async Task<UpdateVoteOutput> Handle(UpdateVoteCommand request, CancellationToken cancellationToken)
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
            throw new Exception($"You can't give vote to poll that is not ongoing");
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
            throw new Exception($"You can't vote this poll because you are not member of the group");
        }

        if (request.ListChoice.Count() > poll.MaximumAnswer || request.ListChoice.Count() <= 0)
        {
            throw new Exception($"Maximum answer is {poll.MaximumAnswer} and at least 1 choice must choosen");
        }

        var isAlreadyFirstTimeVote = await databaseService.Voters
                                .AnyAsync(v => v.PollId == poll.Id && v.CreatedBy == currentUserService.Username);

        if (!isAlreadyFirstTimeVote)
        {
            throw new Exception("You never vote to this poll before");
        }

        var choiceThatAddedByThisUser = await databaseService.Choices
                                        .Where(c => c.CreatedBy == currentUserService.Username && c.PollId == poll.Id && c.IsOther)
                                        .SingleOrDefaultAsync(cancellationToken);

        if (choiceThatAddedByThisUser != null)
        {
            var isUserPickHisChoice = request.ListChoice
                                        .Any(c => c == choiceThatAddedByThisUser.Id);

            if (!isUserPickHisChoice)
            {
                throw new Exception($"You must pick choice you add");
            }
        }

        var voter = await databaseService.Voters
                        .Where(v => v.PollId == request.PollId && v.CreatedBy == currentUserService.Username)
                        .SingleOrDefaultAsync(cancellationToken)
                        ?? throw new Exception("You never vote to this poll before");

        var previousVote = await databaseService.Answers
                                .Where(a => a.VoterId == voter.Id)
                                .ToListAsync(cancellationToken);

        databaseService.Answers.RemoveRange(previousVote);

        var newVoteResult = new List<AnswerItem>();
        foreach (var choice in request.ListChoice)
        {
            var answer = new Answer
            {
                ChoiceId = choice,
                VoterId = voter.Id,
                Created = DateTimeOffset.Now,
                CreatedBy = currentUserService.Username
            };

            newVoteResult.Add(new AnswerItem { AnswerId = answer.Id });

            _ = await databaseService.Answers.AddAsync(answer, cancellationToken);
        }

        _ = await databaseService.SaveAsync(cancellationToken);

        var currentVote = await databaseService.Choices
                                .Include(c => c.Answers)
                                .Where(c => c.PollId == request.PollId)
                                .Select(c => new ChoiceItem
                                {
                                    ChoiceId = c.Id,
                                    Description = c.Description,
                                    NumVote = c.Answers.Count(),
                                })
                                .OrderBy(c => c.Description)
                                .ToListAsync(cancellationToken);

        var jsonVote = JsonConvert.SerializeObject(currentVote);

        await hubContext.Clients.All.SendAsync("SendVote", request.PollId, jsonVote);

        return new UpdateVoteOutput
        {
            Data = newVoteResult
        };
    }
}
