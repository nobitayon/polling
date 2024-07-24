using Delta.Polling.Both.Member.Polls.Commands.UpdatePoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.UpdatePoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record UpdatePollCommand : UpdatePollRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class UpdatePollCommandValidator : AbstractValidator<UpdatePollCommand>
{
    public UpdatePollCommandValidator()
    {
        Include(new UpdatePollRequestValidator());
    }
}

public class UpdatePollCommandHandler(IBackEndService backEndService)
    : IRequestHandler<UpdatePollCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/polls/{request.PollId}", Method.Put);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
