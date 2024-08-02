using Delta.Polling.Both.Member.Choices.Queries.GetChoicesByPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Choices.Queries.GetChoicesByPoll;
[Authorize(RoleName = RoleNameFor.Member)]
public record GetChoicesByPollQuery : GetChoicesByPollRequest, IRequest<ResponseResult<GetChoicesByPollOutput>>
{
}

public class GetChoicesByPollQueryValidator : AbstractValidator<GetChoicesByPollQuery>
{
    public GetChoicesByPollQueryValidator()
    {
        Include(new GetChoicesByPollRequestValidator());
    }
}

public class GetChoicesByPollQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetChoicesByPollQuery, ResponseResult<GetChoicesByPollOutput>>
{
    public async Task<ResponseResult<GetChoicesByPollOutput>> Handle(GetChoicesByPollQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Choices/by-poll-id/{request.PollId}", Method.Get);

        return await backEndService.SendRequestAsync<GetChoicesByPollOutput>(restRequest, cancellationToken);
    }
}
