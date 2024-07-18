using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Google;

public class GoogleEmailService(IOptions<GoogleEmailOptions> options) : IEmailService
{
    public void SendEmail(SendEmailInput input)
    {
        Console.WriteLine($"Ceritanya mengirim email ke {input.To} dengan judul {input.Subject} berisi {input.Body} menggunakan server {options.Value.Server}.");
    }
}
