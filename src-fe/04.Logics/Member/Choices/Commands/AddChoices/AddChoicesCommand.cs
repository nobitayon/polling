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
        var restRequest = new RestRequest("member/Choices", Method.Post);
        //restRequest.AddParameters(request);
        //_ = restRequest.AddHeader("Content-Type", "multipart/form-data");

        //// Add basic fields
        _ = restRequest.AddParameter(nameof(AddChoiceCommand.PollId), request.PollId.ToString());
        _ = restRequest.AddParameter(nameof(AddChoiceCommand.Description), request.Description.ToString());

        // Add media items
        var index = 0;
        foreach (var media in request.MediaRequest)
        {
            if (media.File != null)
            {
                Console.WriteLine("Ada kok masuk sini");
                _ = restRequest.AddFile($"{nameof(AddChoiceCommand.MediaRequest)}[{index}].{nameof(AddChoiceMediaRequest.File)}", media.File.ToBytes(), media.File.FileName, contentType: media.File.ContentType);
                _ = restRequest.AddParameter($"{nameof(AddChoiceCommand.MediaRequest)}[{index}].{nameof(AddChoiceMediaRequest.MediaDescription)}", media.MediaDescription);
            }

            index++;
        }

        Console.WriteLine("haho");
        //var cek = request.MediaRequest.ToList();
        //_ = restRequest.AddFile($"{nameof(AddChoiceCommand.File)})", request.File.ToBytes(), request.File.FileName, contentType: request.File.ContentType);

        return await backEndService.SendRequestAsync<AddChoiceOutput>(restRequest, cancellationToken);
    }
}
