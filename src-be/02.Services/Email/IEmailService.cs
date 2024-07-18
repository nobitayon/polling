namespace Delta.Polling.Services.Email;

public interface IEmailService
{
    void SendEmail(SendEmailInput input);
}

public record SendEmailInput
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
}
