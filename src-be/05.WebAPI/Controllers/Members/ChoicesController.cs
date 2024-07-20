using Delta.Polling.Both.Member.Choices.Commands.AddChoice;
using Delta.Polling.Logics.Member.Choices.Commands.AddChoice;
using Delta.Polling.Logics.Member.Choices.Commands.UpdateChoice;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class ChoicesController : ApiControllerBase
{
    // TODO: Apakah lebih baik dalam PollsController
    [HttpPost]
    public async Task<AddChoiceOutput> AddChoice([FromForm] AddChoiceCommand request)
    {
        return await Sender.Send(request);
    }

    [HttpPut]
    public async Task UpdateChoice([FromForm] UpdateChoiceCommand request)
    {
        await Sender.Send(request);
    }
}
