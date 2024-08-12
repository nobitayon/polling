using Delta.Polling.Both.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.Both.Member.Choices.Commands.AddChoice;
using Delta.Polling.Both.Member.Choices.Queries.GetChoice;
using Delta.Polling.Both.Member.Choices.Queries.GetChoicesByPoll;
using Delta.Polling.Logics.Member.Choices.Commands.AddAnotherChoiceOngoingPoll;
using Delta.Polling.Logics.Member.Choices.Commands.AddChoice;
using Delta.Polling.Logics.Member.Choices.Commands.DeleteChoice;
using Delta.Polling.Logics.Member.Choices.Commands.UpdateChoice;
using Delta.Polling.Logics.Member.Choices.Queries.GetChoice;
using Delta.Polling.Logics.Member.Choices.Queries.GetChoicesByPoll;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class ChoicesController : ApiControllerBase
{
    [HttpGet("by-poll-id/{PollId:guid}")]
    public async Task<GetChoicesByPollOutput> GetChoicesByPoll([FromRoute] GetChoicesByPollQuery request)
    {
        return await Sender.Send(request);
    }

    [HttpGet("{choiceId:guid}")]
    public async Task<GetChoiceOutput> GetChoice([FromRoute] GetChoiceQuery request)
    {
        return await Sender.Send(request);
    }

    // TODO: Apakah lebih baik dalam PollsController
    [HttpPost]
    public async Task<AddChoiceOutput> AddChoice(
        [FromForm] Guid pollId,
        [FromForm] string description,
        [FromForm] IEnumerable<AddChoiceMediaRequest> mediaRequest,
        [FromForm] IFormFile file
        )
    {
        return await Sender.Send(new AddChoiceCommand
        {
            Description = description,
            PollId = pollId,
            MediaRequest = mediaRequest,
            File = file
        });
    }

    [HttpPut]
    public async Task UpdateChoice([FromForm] UpdateChoiceCommand request)
    {
        await Sender.Send(request);
    }

    [HttpDelete("{choiceId:guid}")]
    public async Task DeleteChoice([FromRoute] DeleteChoiceCommand request)
    {
        await Sender.Send(request);
    }

    [HttpPost("another-choice")]
    public async Task<AddAnotherChoiceOngoingPollOutput> AddAnotherChoice([FromForm] AddAnotherChoiceOngoingPollCommand request)
    {
        return await Sender.Send(request);
    }
}
