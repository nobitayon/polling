using Delta.Polling.Both.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetPollQuery : GetPollRequest, IRequest<ResponseResult<GetPollOutput>>
{
}

public class GetPollQueryValidator : AbstractValidator<GetPollQuery>
{
    public GetPollQueryValidator()
    {
        Include(new GetPollRequestValidator());
    }
}

public class GetPollQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetPollQuery, ResponseResult<GetPollOutput>>
{
    public async Task<ResponseResult<GetPollOutput>> Handle(GetPollQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Polls/{request.PollId}", Method.Get);

        return await backEndService.SendRequestAsync<GetPollOutput>(restRequest, cancellationToken);
    }
}

