using Delta.Polling.Both.Admin.Groups.Commands.RemoveMember;

namespace Delta.Polling.Logics.Admin.Groups.Commands.RemoveMember;

[Authorize(RoleName = RoleNameFor.Administrator)]
public class RemoveMemberCommand : RemoveMemberRequest, IRequest
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
    ICurrentUserService currentUserService)
    : IRequestHandler<RemoveMemberCommand>
{
    public async Task Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        foreach (var username in request.ListMemberUsername)
        {
            var groupMember = await databaseService.GroupMembers
                                .Where(gm => gm.Username == username && gm.GroupId == request.GroupId)
                                .SingleOrDefaultAsync()
                                ?? throw new Exception($"{username} is not member of group");

            _ = databaseService.GroupMembers.Remove(groupMember);
        }

        _ = await databaseService.SaveAsync(cancellationToken);
    }
}

