using Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

namespace Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

[Authorize(RoleName = RoleNameFor.Admin)]
public record GetUsersNotMemberFromGroupQuery : GetUsersNotMemberFromGroupRequest, IRequest<ResponseResult<GetUsersNotMemberFromGroupOutput>>
{
}

public class GetUsersNotMemberFromGroupQueryValidator : AbstractValidator<ResponseResult<GetUsersNotMemberFromGroupQuery>>
{
    public GetUsersNotMemberFromGroupQueryValidator()
    {
        Include(new GetUsersNotMemberFromGroupQueryValidator());
    }
}

public class GetUsersNotMemberFromGroupQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetUsersNotMemberFromGroupQuery, ResponseResult<GetUsersNotMemberFromGroupOutput>>
{
    public async Task<ResponseResult<GetUsersNotMemberFromGroupOutput>> Handle(GetUsersNotMemberFromGroupQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"admin/groups/{request.GroupId}/not-member", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetUsersNotMemberFromGroupOutput>(restRequest, cancellationToken);
    }
}
