using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;
using Delta.Polling.Logics.Member.Groups.Queries.GetMyGroups;

namespace Delta.Polling.WebAPI.Controllers.Members;

[Route("api/Member/[controller]")]
public class GroupsController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetMyGroupsOutput> GetMyGroups([FromQuery] GetMyGroupsQuery request)
    {
        return await Sender.Send(request);
    }
}

