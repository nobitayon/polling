using Delta.Polling.Both.Member.Polls.Commands.StartPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.StartPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record StartPollCommand : StartPollRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class StartPollCommandValidator : AbstractValidator<StartPollCommand>
{
    public StartPollCommandValidator()
    {
        Include(new StartPollRequestValidator());
    }
}

public class StartPollCommandHandler(IBackEndService backEndService)
    : IRequestHandler<StartPollCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(StartPollCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("HERE");
        Console.WriteLine($"{request.PollId}");
        var restRequest = new RestRequest($"member/polls/{request.PollId}/start", Method.Patch);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}

