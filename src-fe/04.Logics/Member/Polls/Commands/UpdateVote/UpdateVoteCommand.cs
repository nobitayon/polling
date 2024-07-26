using Delta.Polling.Both.Member.Polls.Commands.UpdateVote;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.UpdateVote;

[Authorize(RoleName = RoleNameFor.Member)]
public record UpdateVoteCommand : UpdateVoteRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class UpdateVoteCommandValidator : AbstractValidator<UpdateVoteCommand>
{
    public UpdateVoteCommandValidator()
    {
        Include(new UpdateVoteRequestValidator());
    }
}

public class UpdateVoteCommandHandler(IBackEndService backEndService)
    : IRequestHandler<UpdateVoteCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(UpdateVoteCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/polls/{request.PollId}/update-vote", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
