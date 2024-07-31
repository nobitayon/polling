using Delta.Polling.Both.Admin.Groups.Commands.AddGroup;

namespace Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.AddGroup;

[Authorize(RoleName = RoleNameFor.Admin)]
public record AddGroupCommand : AddGroupRequest, IRequest<ResponseResult<AddGroupOutput>>
{
}

public class AddGroupCommandValidator : AbstractValidator<AddGroupCommand>
{
    public AddGroupCommandValidator()
    {
        Include(new AddGroupRequestValidator());
    }
}

public class AddGroupCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddGroupCommand, ResponseResult<AddGroupOutput>>
{
    public async Task<ResponseResult<AddGroupOutput>> Handle(AddGroupCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"admin/groups", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddGroupOutput>(restRequest, cancellationToken);
    }
}
