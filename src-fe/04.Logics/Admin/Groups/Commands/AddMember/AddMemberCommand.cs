using Delta.Polling.Both.Admin.Groups.Commands.AddMember;

namespace Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.AddMember;

[Authorize(RoleName = RoleNameFor.Administrator)]
public record AddMemberCommand : AddMemberRequest, IRequest<ResponseResult<AddMemberOutput>>
{
}

public class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        Include(new AddMemberRequestValidator());
    }
}

public class AddMemberCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddMemberCommand, ResponseResult<AddMemberOutput>>
{
    public async Task<ResponseResult<AddMemberOutput>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"admin/groups/{request.GroupId}/add-member", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddMemberOutput>(restRequest, cancellationToken);
    }
}
