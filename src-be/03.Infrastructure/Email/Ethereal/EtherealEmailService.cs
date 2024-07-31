using Delta.Polling.Services.Email;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace Delta.Polling.Infrastructure.Email.Ethereal;

public class EtherealEmailService(
    IOptions<EmailOptions> emailOptions,
    IOptions<EtherealEmailOptions> etherealEmailOptions,
    ILogger<EtherealEmailService> logger)
    : IEmailService
{
    private readonly EmailOptions _emailOptions = emailOptions.Value;
    private readonly EtherealEmailOptions _etherealEmailOptions = etherealEmailOptions.Value;

    public void SendEmail(SendEmailInput input)
    {
        var toAddresses = string.Join(',', input.Tos.Select(x => x.Address));

        logger.LogInformation("Attempting to send email to {ToAddresses} using provider {EmailProvider}.", toAddresses, EmailProvider.Ethereal);

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = input.Body.Replace("{{LinkBaseUrl}}", _emailOptions.LinkBaseUrl)
        };

        logger.LogInformation("From: {@From}", _emailOptions.From);

        var message = new MimeMessage
        {
            Subject = input.Subject,
            From = { new MailboxAddress(_emailOptions.From.Name, _emailOptions.From.Address) },
            Body = bodyBuilder.ToMessageBody()
        };

        foreach (var mailBox in input.Tos)
        {
            message.To.Add(new MailboxAddress(mailBox.Name, mailBox.Address));
        }

        foreach (var mailBox in input.Ccs)
        {
            message.Cc.Add(new MailboxAddress(mailBox.Name, mailBox.Address));
        }

        using var smtpClient = new SmtpClient();

        smtpClient.Connect(_etherealEmailOptions.Server, _etherealEmailOptions.Port, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(_etherealEmailOptions.Username, _etherealEmailOptions.Password);
        _ = smtpClient.Send(message);
        smtpClient.Disconnect(true);
    }
}
