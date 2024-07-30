using Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;

namespace Delta.Polling.FrontEnd.Logics.Member.Groups.Queries.GetMyGroup;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyGroupQuery : GetMyGroupRequest, IRequest<ResponseResult<GetMyGroupOutput>>
{
}

public class GetMyGroupQueryValidator : AbstractValidator<ResponseResult<GetMyGroupQuery>>
{
    public GetMyGroupQueryValidator()
    {
        Include(new GetMyGroupQueryValidator());
    }
}

public class GetMyGroupQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMyGroupQuery, ResponseResult<GetMyGroupOutput>>
{
    public async Task<ResponseResult<GetMyGroupOutput>> Handle(GetMyGroupQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/groups/{request.GroupId}", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetMyGroupOutput>(restRequest, cancellationToken);
    }
}
