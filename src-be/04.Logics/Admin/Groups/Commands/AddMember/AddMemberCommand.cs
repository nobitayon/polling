using Delta.Polling.Both.Admin.Groups.Commands.AddMember;
using Delta.Polling.Domain.Groups.Entities;

namespace Delta.Polling.Logics.Admin.Groups.Commands.AddMember;

[Authorize(RoleName = RoleNameFor.Administrator)]
public record AddMemberCommand : AddMemberRequest, IRequest<AddMemberOutput>
{
}

public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        Include(new AddMemberRequestValidator());
    }
}

public class AddMemberCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddMemberCommand, AddMemberOutput>
{
    public async Task<AddMemberOutput> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        //var listGroupMember = new List<Guid>();
        //foreach (var username in request.ListMemberUsername)
        //{
        //    var groupMember = new GroupMember
        //    {
        //        GroupId = request.GroupId,
        //        Username = username,
        //        Created = DateTimeOffset.Now,
        //        CreatedBy = currentUserService.Username
        //    };

        //    _ = await databaseService.GroupMembers.AddAsync(groupMember, cancellationToken);

        //    listGroupMember.Add(groupMember.Id);
        //}

        var member = await databaseService.GroupMembers
                            .Where(gm => gm.GroupId == request.GroupId && gm.Username == request.Username)
                            .SingleOrDefaultAsync(cancellationToken);

        if (member != null)
        {
            throw new Exception($"{request.Username} already in group");
        }

        var groupMember = new GroupMember
        {
            GroupId = request.GroupId,
            Username = request.Username,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username
        };

        _ = await databaseService.GroupMembers.AddAsync(groupMember, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddMemberOutput
        {
            Data = new AddMemberResult
            {
                GroupMemberId = groupMember.Id
            }
        };
    }
}

