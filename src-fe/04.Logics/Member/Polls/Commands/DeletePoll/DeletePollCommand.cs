using Delta.Polling.Both.Member.Polls.Commands.DeletePoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.DeletePoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record DeletePollCommand : DeletePollRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class DeletePollCommandValidator : AbstractValidator<DeletePollCommand>
{
    public DeletePollCommandValidator()
    {
        Include(new DeletePollRequestValidator());
    }
}

public class DeletePollCommandHandler(IBackEndService backEndService)
    : IRequestHandler<DeletePollCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(DeletePollCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/polls/{request.PollId}", Method.Delete);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
