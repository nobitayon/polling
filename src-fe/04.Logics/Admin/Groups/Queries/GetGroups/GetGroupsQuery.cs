using Delta.Polling.Both.Admin.Groups.Queries.GetGroups;

namespace Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroups;

[Authorize(RoleName = RoleNameFor.Admin)]
public record GetGroupsQuery : GetGroupsRequest, IRequest<ResponseResult<GetGroupsOutput>>
{
}

public class GetGroupsQueryValidator : AbstractValidator<ResponseResult<GetGroupsQuery>>
{
    public GetGroupsQueryValidator()
    {
        Include(new GetGroupsQueryValidator());
    }
}

public class GetGroupsQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetGroupsQuery, ResponseResult<GetGroupsOutput>>
{
    public async Task<ResponseResult<GetGroupsOutput>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("admin/groups", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetGroupsOutput>(restRequest, cancellationToken);
    }
}
