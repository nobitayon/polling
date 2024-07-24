using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetMyPolls;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetMyPollsQuery : GetMyPollsRequest, IRequest<ResponseResult<GetMyPollsOutput>>
{
}

public class GetMyPollsQueryValidator : AbstractValidator<ResponseResult<GetMyPollsQuery>>
{
    public GetMyPollsQueryValidator()
    {
        Include(new GetMyPollsQueryValidator());
    }
}

public class GetMyPollsQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetMyPollsQuery, ResponseResult<GetMyPollsOutput>>
{
    public async Task<ResponseResult<GetMyPollsOutput>> Handle(GetMyPollsQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("member/polls", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetMyPollsOutput>(restRequest, cancellationToken);
    }
}
