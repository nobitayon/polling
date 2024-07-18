using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email.Dummy;

public class DummyEmailService : IEmailService
{
    public void SendEmail(SendEmailInput input)
    {
        Console.WriteLine($"Ceritanya mengirim email ke {input.To} dengan judul {input.Subject} berisi {input.Body} menggunakan Dummy provider.");
    }
}
