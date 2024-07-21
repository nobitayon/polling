using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Member.Polls.Commands.AddVote;
using Delta.Polling.Domain.Answers.Entities;
using Delta.Polling.Domain.Voters.Entities;

namespace Delta.Polling.Logics.Member.Polls.Commands.AddVote;

[Authorize(RoleName = RoleNameFor.Member)]
public class AddVoteCommand : AddVoteRequest, IRequest<AddVoteOutput>
{
}

public class AddVoteCommandValidator : AbstractValidator<AddVoteCommand>
{
    public AddVoteCommandValidator()
    {
        Include(new AddVoteRequestValidator());
    }
}

// TODO: Mikirin lagi apakah dengan membedakan endpoint saat orang submit dan resubmit(ketika mungkin ingin ubah vote) itu perlu atau tidak
public class AddVoteCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddVoteCommand, AddVoteOutput>
{
    public async Task<AddVoteOutput> Handle(AddVoteCommand request, CancellationToken cancellationToken)
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
            throw new ForbiddenException($"You can't vote this poll because you are not member of the group");
        }

        if (request.ListChoice.Count() > poll.MaximumAnswer || request.ListChoice.Count() <= 0)
        {
            throw new ForbiddenException($"Maximum answer is {poll.MaximumAnswer} and at least 1 choice must choosen");
        }

        var isAlreadyFirstTimeVote = await databaseService.Voters
                                .AnyAsync(v => v.PollId == poll.Id && v.CreatedBy == currentUserService.Username);

        if (isAlreadyFirstTimeVote)
        {
            throw new Exception("You can't submit this vote using this endpoint, but you can try resubmit endpoint to change your vote");
        }

        var choiceThatAddedByThisUser = await databaseService.Choices
                                        .Where(c => c.CreatedBy == currentUserService.Username && c.IsOther)
                                        .SingleOrDefaultAsync(cancellationToken);

        if (choiceThatAddedByThisUser != null)
        {
            var isUserPickHisChoice = request.ListChoice
                                        .Any(c => c == choiceThatAddedByThisUser.Id);

            if (!isUserPickHisChoice)
            {
                throw new ForbiddenException($"You must pick choice you add");
            }
        }

        var voter = new Voter
        {
            PollId = poll.Id,
            Username = currentUserService.Username,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username,
        };

        _ = await databaseService.Voters.AddAsync(voter, cancellationToken);

        var listAnswer = new List<Answer>();
        var addVoteResult = new List<AnswerItem>();
        foreach (var choice in request.ListChoice)
        {
            var answer = new Answer
            {
                ChoiceId = choice,
                VoterId = voter.Id,
                Created = DateTimeOffset.Now,
                CreatedBy = currentUserService.Username
            };

            addVoteResult.Add(new AnswerItem { AnswerId = answer.Id });

            _ = await databaseService.Answers.AddAsync(answer, cancellationToken);
        }

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddVoteOutput
        {
            Data = addVoteResult
        };
    }
}

