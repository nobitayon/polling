using Delta.Polling.Both.Member.Polls.Commands.AddVote;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddVote;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddVoteCommand : AddVoteRequest, IRequest<ResponseResult<AddVoteOutput>>
{
}

public class AddVoteCommandValidator : AbstractValidator<AddVoteCommand>
{
    public AddVoteCommandValidator()
    {
        Include(new AddVoteRequestValidator());
    }
}

public class AddVoteCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddVoteCommand, ResponseResult<AddVoteOutput>>
{
    public async Task<ResponseResult<AddVoteOutput>> Handle(AddVoteCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/polls/{request.PollId}/vote", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddVoteOutput>(restRequest, cancellationToken);
    }
}
