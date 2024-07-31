using Delta.Polling.Both.Emails.Commands.SendTestEmail;
using Delta.Polling.Services.Email;

namespace Delta.Polling.Logics.Emails.Commands.SendTestEmail;

[Authorize]
public record SendTestEmailCommand : SendTestEmailRequest, IRequest
{
}

public class SendTestEmailCommandValidator : AbstractValidator<SendTestEmailCommand>
{
    public SendTestEmailCommandValidator()
    {
        Include(new SendTestEmailRequestValidator());
    }
}

public class SendTestEmailCommandHandler(IEmailService emailService)
    : IRequestHandler<SendTestEmailCommand>
{
    public Task Handle(SendTestEmailCommand request, CancellationToken cancellationToken)
    {
        var tos = request.Tos.Select(address => new MailBoxModel
        {
            Name = address,
            Address = address
        });

        var ccs = request.Ccs.Select(address => new MailBoxModel
        {
            Name = address,
            Address = address
        });

        var input = new SendEmailInput
        {
            Tos = tos,
            Ccs = ccs,
            Subject = "Test Email",
            Body = request.Notes
        };

        emailService.SendEmail(input);

        return Task.CompletedTask;
    }
}
