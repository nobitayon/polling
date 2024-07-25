using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;

namespace Delta.Polling.FrontEnd.Logics.Member.Groups.Queries.GetMyGroups;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyGroupsQuery : GetMyGroupsRequest, IRequest<ResponseResult<GetMyGroupsOutput>>
{
}

public class GetMyGroupsQueryValidator : AbstractValidator<ResponseResult<GetMyGroupsQuery>>
{
    public GetMyGroupsQueryValidator()
    {
        Include(new GetMyGroupsQueryValidator());
    }
}

public class GetMyGroupsQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMyGroupsQuery, ResponseResult<GetMyGroupsOutput>>
{
    public async Task<ResponseResult<GetMyGroupsOutput>> Handle(GetMyGroupsQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("member/groups", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetMyGroupsOutput>(restRequest, cancellationToken);
    }
}
