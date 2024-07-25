using Delta.Polling.Both.Member.Polls.Commands.AddPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddPollCommand : AddPollRequest, IRequest<ResponseResult<AddPollOutput>>
{
}

public class AddPollCommandValidator : AbstractValidator<AddPollCommand>
{
    public AddPollCommandValidator()
    {
        Include(new AddPollRequestValidator());
    }
}

public class AddPollCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddPollCommand, ResponseResult<AddPollOutput>>
{
    public async Task<ResponseResult<AddPollOutput>> Handle(AddPollCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/polls", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddPollOutput>(restRequest, cancellationToken);
    }
}
