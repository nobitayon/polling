using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Admin.Groups.Commands.RemoveMember;
using Delta.Polling.Domain.Polls.Entities;
using Delta.Polling.Logics.SignalR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Delta.Polling.Logics.Admin.Groups.Commands.RemoveMember;

[Authorize(RoleName = RoleNameFor.Admin)]
public record RemoveMemberCommand : RemoveMemberRequest, IRequest
{
}

public class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
{
    public RemoveMemberCommandValidator()
    {
        Include(new RemoveMemberRequestValidator());
    }
}

public class RemoveMemberCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService,
    IHubContext<PollHub> hubContext)
    : IRequestHandler<RemoveMemberCommand>
{
    public async Task Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var groupMember = await databaseService.GroupMembers
                                .Where(gm => gm.Username == request.Username && gm.GroupId == request.GroupId)
                                .SingleOrDefaultAsync()
                                ?? throw new Exception($"{request.Username} is not member of group");

        _ = databaseService.GroupMembers.Remove(groupMember);

        var currentOngoingPoll = await databaseService.Polls
                                                .Where(p => p.GroupId == request.GroupId && p.Status == PollStatus.Ongoing)
                                                .Include(p => p.Voters)
                                                .ThenInclude(v => v.Answers)
                                                .Select(p => new
                                                {
                                                    PollId = p.Id,
                                                    Title = p.Title,
                                                    Question = p.Question,
                                                    CreatedBy = p.CreatedBy,
                                                    Voter = p.Voters.Where(v => v.Username == request.Username)
                                                    .Select(v => new
                                                    {
                                                        VoterId = v.Id,
                                                        Answers = v.Answers.ToList()
                                                    })
                                                    .SingleOrDefault()
                                                })
                                                .ToListAsync(cancellationToken);

        var listPollId = new List<Guid>();
        foreach (var poll in currentOngoingPoll)
        {
            if (poll.CreatedBy == request.Username)
            {
                var updatedPoll = await databaseService.Polls.Where(p => p.Id == poll.PollId)
                                        .SingleOrDefaultAsync(cancellationToken)
                                        ?? throw new EntityNotFoundException(nameof(Poll), poll.PollId);

                updatedPoll.Status = PollStatus.Abandoned;
            }
            else
            {
                if (poll.Voter != null)
                {
                    if (poll.Voter.Answers.Count() > 0)
                    {
                        listPollId.Add(poll.PollId);
                    }

                    foreach (var answer in poll.Voter.Answers)
                    {
                        _ = databaseService.Answers.Remove(answer);
                    }

                    var voter = await databaseService.Voters.Where(v => v.Id == poll.Voter.VoterId)
                                        .SingleOrDefaultAsync(cancellationToken)
                                        ?? throw new Exception("There should be voter, but voter not found");

                    _ = databaseService.Voters.Remove(voter);
                }
            }
        }

        _ = await databaseService.SaveAsync(cancellationToken);

        // Optional untuk live
        foreach (var pollId in listPollId)
        {
            var currentVote = await databaseService.Choices
                                .Include(c => c.Answers)
                                .Where(c => c.PollId == pollId)
                                .Select(c => new
                                {
                                    ChoiceId = c.Id,
                                    Description = c.Description,
                                    NumVote = c.Answers.Count(),
                                })
                                .OrderBy(c => c.Description)
                                .ToListAsync(cancellationToken);

            var jsonVote = JsonConvert.SerializeObject(currentVote);

            await hubContext.Clients.All.SendAsync("SendVote", pollId, jsonVote);
        }
    }
}

