using Delta.Polling.Both.Admin.Groups.Queries.GetGroup;

namespace Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroup;

[Authorize(RoleName = RoleNameFor.Administrator)]
public record GetGroupQuery : GetGroupRequest, IRequest<ResponseResult<GetGroupOutput>>
{
}

public class GetGroupQueryValidator : AbstractValidator<ResponseResult<GetGroupQuery>>
{
    public GetGroupQueryValidator()
    {
        Include(new GetGroupQueryValidator());
    }
}

public class GetGroupQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetGroupQuery, ResponseResult<GetGroupOutput>>
{
    public async Task<ResponseResult<GetGroupOutput>> Handle(GetGroupQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.GroupId);
        Console.WriteLine(request.Page);
        Console.WriteLine(request.PageSize);
        var restRequest = new RestRequest($"admin/groups/{request.GroupId}", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetGroupOutput>(restRequest, cancellationToken);
    }
}
