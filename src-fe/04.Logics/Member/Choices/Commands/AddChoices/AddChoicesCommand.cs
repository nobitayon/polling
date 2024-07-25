using Delta.Polling.Both.Member.Choices.Commands.AddChoice;

namespace Delta.Polling.FrontEnd.Logics.Member.Choices.Commands.AddChoices;

[Authorize(RoleName = RoleNameFor.Member)]
public record AddChoiceCommand : AddChoiceRequest, IRequest<ResponseResult<AddChoiceOutput>>
{
}

public class AddChoiceCommandValidator : AbstractValidator<AddChoiceCommand>
{
    public AddChoiceCommandValidator()
    {
        Include(new AddChoiceRequestValidator());
    }
}

public class AddChoiceCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddChoiceCommand, ResponseResult<AddChoiceOutput>>
{
    public async Task<ResponseResult<AddChoiceOutput>> Handle(AddChoiceCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("member/choices", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddChoiceOutput>(restRequest, cancellationToken);
    }
}
