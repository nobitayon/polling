using Delta.Polling.Both.Admin.Groups.Commands.RemoveMember;

namespace Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.RemoveMember;

[Authorize(RoleName = RoleNameFor.Admin)]
public record RemoveMemberCommand : RemoveMemberRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
{
    public RemoveMemberCommandValidator()
    {
        Include(new RemoveMemberRequestValidator());
    }
}

public class RemoveMemberCommandHandler(IBackEndService backEndService)
    : IRequestHandler<RemoveMemberCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"admin/groups/{request.GroupId}/remove-member", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
