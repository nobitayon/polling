using Delta.Polling.Both.Member.Polls.Commands.AddPoll;
using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.Both.Member.Polls.Queries.GetPoll;
using Delta.Polling.Logics.Member.Polls.Commands.AddPoll;
using Delta.Polling.Logics.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.Logics.Member.Polls.Queries.GetPoll;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class PollsController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetMyPollsOutput> GetMyPolls([FromQuery] GetMyPollsQuery request)
    {
        return await Sender.Send(request);
    }

    [HttpGet("{pollId:guid}")]
    public async Task<GetPollOutput> GetPoll([FromRoute] Guid pollId)
    {
        var request = new GetPollQuery { PollId = pollId };

        return await Sender.Send(request);
    }

    [HttpPost]
    public async Task<AddPollOutput> AddPoll([FromForm] AddPollCommand request)
    {
        return await Sender.Send(request);
    }
}
