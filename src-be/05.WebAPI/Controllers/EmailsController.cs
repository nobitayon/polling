using Delta.Polling.Logics.Emails.Commands.SendTestEmail;

namespace Delta.Polling.WebAPI.Controllers.Contributors;

public class EmailsController : ApiControllerBase
{
    [HttpPost("Test")]
    public async Task SendTestEmail([FromForm] SendTestEmailCommand command)
    {
        await Sender.Send(command);
    }
}
