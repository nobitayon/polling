using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Microsoft;

public class MicrosoftEmailService(IOptions<MicrosoftEmailOptions> options) : IEmailService
{
    public void SendEmail(SendEmailInput input)
    {
        Console.WriteLine($"Ceritanya mengirim email ke {input.To} dengan judul {input.Subject} berisi {input.Body} menggunakan API pada URL {options.Value.ApiUrl}.");
    }
}
