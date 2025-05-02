namespace Delta.Polling.Services.Email;

public interface IEmailService
{
    public void SendEmail(SendEmailInput input);
}

public record SendEmailInput
{
    public required IEnumerable<MailBoxModel> Tos { get; init; }
    public required IEnumerable<MailBoxModel> Ccs { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}

public record MailBoxModel
{
    public required string Name { get; init; }
    public required string Address { get; init; }
}
