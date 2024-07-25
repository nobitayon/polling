using Delta.Polling.Both.Member.Polls.Queries.GetPollWithAllAnswer;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetPollWithAllAnswerQuery;

[Authorize(RoleName = RoleNameFor.Member)]
public record GetPollWithAllAnswerQuery : GetPollWithAllAnswerRequest, IRequest<ResponseResult<GetPollWithAllAnswerOutput>>
{
}

public class GetPollWithAllAnswerQueryValidator : AbstractValidator<GetPollWithAllAnswerQuery>
{
    public GetPollWithAllAnswerQueryValidator()
    {
        Include(new GetPollWithAllAnswerRequestValidator());
    }
}

public class GetPollWithAllAnswerQueryHandler(IBackEndService backEndService)
    : IRequestHandler<GetPollWithAllAnswerQuery, ResponseResult<GetPollWithAllAnswerOutput>>
{
    public async Task<ResponseResult<GetPollWithAllAnswerOutput>> Handle(GetPollWithAllAnswerQuery request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"Member/Polls/{request.PollId}/answer-detail", Method.Get);

        return await backEndService.SendRequestAsync<GetPollWithAllAnswerOutput>(restRequest, cancellationToken);
    }
}
