using Delta.Polling.Both.Member.Polls.Queries.GetRecentParticipatedPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetRecentParticipatedPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetRecentParticipatedPollQuery : GetRecentParticipatedPollRequest, IRequest<ResponseResult<GetRecentParticipatedPollOutput>>
{
}

public class GetRecentParticipatedPollQueryValidator : AbstractValidator<GetRecentParticipatedPollQuery>
{
    public GetRecentParticipatedPollQueryValidator()
    {
        Include(new GetRecentParticipatedPollRequestValidator());
    }
}

public class GetRecentParticipatedPollQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetRecentParticipatedPollQuery, ResponseResult<GetRecentParticipatedPollOutput>>
{
    public async Task<ResponseResult<GetRecentParticipatedPollOutput>> Handle(GetRecentParticipatedPollQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Polls/recent-poll", Method.Get);

        return await backEndService.SendRequestAsync<GetRecentParticipatedPollOutput>(restRequest, cancellationToken);
    }
}
