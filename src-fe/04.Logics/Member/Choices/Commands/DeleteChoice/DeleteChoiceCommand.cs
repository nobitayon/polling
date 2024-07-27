using Delta.Polling.Both.Member.Choices.Commands.DeleteChoice;

namespace Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.DeleteChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record DeleteChoiceCommand : DeleteChoiceRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class DeleteChoiceCommandValidator : AbstractValidator<DeleteChoiceCommand>
{
    public DeleteChoiceCommandValidator()
    {
        Include(new DeleteChoiceRequestValidator());
    }
}

public class DeleteChoiceCommandHandler(IBackEndService backEndService)
    : IRequestHandler<DeleteChoiceCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(DeleteChoiceCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest($"member/choices/{request.ChoiceId}", Method.Delete);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
