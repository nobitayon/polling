using Delta.Polling.Both.Member.ChoiceMedias.AddChoiceMedia;

namespace Delta.Polling.FrontEnd.Logics.Member.ChoiceMedias.Commands.AddChoiceMedia;

public record AddChoiceMediaCommand : AddChoiceMediaRequest, IRequest<ResponseResult<AddChoiceMediaOutput>>
{
}

public class AddChoiceMediaCommandValidator : AbstractValidator<AddChoiceMediaCommand>
{
    public AddChoiceMediaCommandValidator()
    {
        Include(new AddChoiceMediaRequestValidator());
    }
}

public class AddChoiceMediaCommandHandler(IBackEndService backEndService)
    : IRequestHandler<AddChoiceMediaCommand, ResponseResult<AddChoiceMediaOutput>>
{
    public async Task<ResponseResult<AddChoiceMediaOutput>> Handle(AddChoiceMediaCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("Member/ChoiceMedias", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<AddChoiceMediaOutput>(restRequest, cancellationToken);
    }
}
