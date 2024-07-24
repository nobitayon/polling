using Delta.Polling.Both.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;

namespace Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddAnotherChoiceOngoingPollCommand : AddAnotherChoiceOngoingPollRequest, IRequest<ResponseResult<AddAnotherChoiceOngoingPollOutput>>
{
}

public class AddAnotherChoiceOngoingPollCommandValidator : AbstractValidator<AddAnotherChoiceOngoingPollCommand>
{
    public AddAnotherChoiceOngoingPollCommandValidator()
    {
        Include(new AddAnotherChoiceOngoingPollRequestValidator());
    }
}

public class AddAnotherChoiceOngoingPollCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddAnotherChoiceOngoingPollCommand, ResponseResult<AddAnotherChoiceOngoingPollOutput>>
{
    public async Task<ResponseResult<AddAnotherChoiceOngoingPollOutput>> Handle(AddAnotherChoiceOngoingPollCommand request, CancellationToken cancellationToken)
    {
        //var restRequest = new RestRequest("member/Choices", Method.Post);
        var restRequest = new RestRequest("member/choices/another-choice", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddAnotherChoiceOngoingPollOutput>(restRequest, cancellationToken);
    }
}
