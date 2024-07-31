using Delta.Polling.Both.Emails.Commands.SendTestEmail;

namespace Delta.Polling.FrontEnd.Logics.Emails.Commands.SendTestEmail;

[Authorize]
public record SendTestEmailCommand : SendTestEmailRequest, IRequest<ResponseResult<NoContentResponse>>
{
}

public class SendTestEmailCommandValidator : AbstractValidator<SendTestEmailCommand>
{
    public SendTestEmailCommandValidator()
    {
        Include(new SendTestEmailRequestValidator());
    }
}

public class SendTestEmailCommandHandler(IBackEndService backEndService)
    : IRequestHandler<SendTestEmailCommand, ResponseResult<NoContentResponse>>
{
    public async Task<ResponseResult<NoContentResponse>> Handle(SendTestEmailCommand request, CancellationToken cancellationToken)
    {
        var restRequest = new RestRequest("Emails/Test", Method.Post);
        restRequest.AddParameters(request);

        return await backEndService.SendRequestAsync<NoContentResponse>(restRequest, cancellationToken);
    }
}
