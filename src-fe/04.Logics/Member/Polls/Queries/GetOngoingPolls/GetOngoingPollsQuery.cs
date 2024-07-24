using Delta.Polling.Both.Member.Polls.Queries.GetOngoingPolls;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetOngoingPolls;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetOngoingPollsQuery : GetOngoingPollsRequest, IRequest<ResponseResult<GetOngoingPollsOutput>>
{
}

public class GetOngoingPollsQueryValidator : AbstractValidator<ResponseResult<GetOngoingPollsQuery>>
{
    public GetOngoingPollsQueryValidator()
    {
        Include(new GetOngoingPollsQueryValidator());
    }
}

public class GetOngoingPollsQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetOngoingPollsQuery, ResponseResult<GetOngoingPollsOutput>>
{
    public async Task<ResponseResult<GetOngoingPollsOutput>> Handle(GetOngoingPollsQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("Member/Polls/ongoing", Method.Get);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<GetOngoingPollsOutput>(restRequest, cancellationToken);
    }
}
