using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.Logics.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class PollsController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetMyPollsOutput> GetMyPolls([FromQuery] GetMyPollsQuery request)
    {
        return await Sender.Send(request);
    }
}
