using Delta.Polling.Both.Member.Polls.Commands.FinishPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.FinishPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record FinishPollCommand : FinishPollRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class FinishPollCommandValidator : AbstractValidator<FinishPollCommand>
{
    public FinishPollCommandValidator()
    {
        Include(new FinishPollRequestValidator());
    }
}

public class FinishPollCommandHandler(IBackEndService backEndService)
    : IRequestHandler<FinishPollCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(FinishPollCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/polls/{request.PollId}/finish", Method.Patch);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
