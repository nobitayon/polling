using Delta.Polling.Both.Member.Polls.Queries.GetRecentGeneral;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetRecentGeneral;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetRecentGeneralQuery : GetRecentGeneralRequest, IRequest<ResponseResult<GetRecentGeneralOutput>>
{
}

public class GetRecentGeneralQueryValidator : AbstractValidator<GetRecentGeneralQuery>
{
    public GetRecentGeneralQueryValidator()
    {
        Include(new GetRecentGeneralRequestValidator());
    }
}

public class GetRecentGeneralQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetRecentGeneralQuery, ResponseResult<GetRecentGeneralOutput>>
{
    public async Task<ResponseResult<GetRecentGeneralOutput>> Handle(GetRecentGeneralQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Polls/recent-general-poll", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetRecentGeneralOutput>(restRequest, cancellationToken);
    }
}
