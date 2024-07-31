using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Dummy;

public class DummyEmailService(ILogger<DummyEmailService> logger) : IEmailService
{
    public void SendEmail(SendEmailInput input)
    {
        var toAddresses = string.Join(',', input.Tos.Select(x => x.Address));

        logger.LogInformation("Attempting to send email to {ToAddresses} using provider {EmailProvider}.", toAddresses, EmailProvider.Dummy);
    }
}
