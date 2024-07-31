using Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;
using Delta.Polling.Logics.Member.Votes.Queries.GetMyVotes;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class VotesController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetMyVotesOutput> GetMyVotes([FromQuery] GetMyVotesQuery request)
    {
        return await Sender.Send(request);
    }
}
