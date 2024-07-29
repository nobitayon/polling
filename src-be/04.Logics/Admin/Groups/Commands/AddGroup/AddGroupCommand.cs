using Delta.Polling.Both.Admin.Groups.Commands.AddGroup;
using Delta.Polling.Domain.Groups.Entities;

namespace Delta.Polling.Logics.Admin.Groups.Commands.AddGroup;

[Authorize(RoleName = RoleNameFor.Administrator)]
public record AddGroupCommand : AddGroupRequest, IRequest<AddGroupOutput>
{
}

public class AddGroupCommandValidator : AbstractValidator<AddGroupCommand>
{
    public AddGroupCommandValidator()
    {
        Include(new AddGroupRequestValidator());
    }
}

public class AddGroupCommandHandler(
    IDatabaseService databaseService,
    ICurrentUserService currentUserService)
    : IRequestHandler<AddGroupCommand, AddGroupOutput>
{
    public async Task<AddGroupOutput> Handle(AddGroupCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.Username))
        {
            throw new Exception("User is not authenticated.");
        }

        var group = new Group
        {
            Name = request.Name,
            Created = DateTimeOffset.Now,
            CreatedBy = currentUserService.Username
        };

        _ = await databaseService.Groups.AddAsync(group, cancellationToken);

        _ = await databaseService.SaveAsync(cancellationToken);

        return new AddGroupOutput
        {
            Data = new AddGroupResult
            {
                GroupId = group.Id
            }
        };
    }
}
