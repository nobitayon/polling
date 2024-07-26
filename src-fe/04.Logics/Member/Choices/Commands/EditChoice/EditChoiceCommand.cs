using Delta.Polling.Both.Member.Choices.Commands.UpdateChoice;

namespace Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.EditChoice;

[Authorize(RoleName = RoleNameFor.Member)]
public record UpdateChoiceCommand : UpdateChoiceRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class UpdateChoiceCommandValidator : AbstractValidator<UpdateChoiceCommand>
{
    public UpdateChoiceCommandValidator()
    {
        Include(new UpdateChoiceRequestValidator());
    }
}

public class UpdateChoiceCommandHandler(IBackEndService backEndService)
    : IRequestHandler<UpdateChoiceCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(UpdateChoiceCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("member/choices", Method.Put);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
