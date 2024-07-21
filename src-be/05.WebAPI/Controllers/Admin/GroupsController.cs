using Delta.Polling.Both.Admin.Groups.Queries.GetGroups;
using Delta.Polling.Logics.Admin.Groups.Queries.GetGroups;

namespace Delta.Polling.WebAPI.Controllers.Admin;

[Route("api/Admin/[controller]")]
public class GroupsController : ApiControllerBase
{
    [HttpGet]
    public async Task<GetGroupsOutput> GetGroups([FromQuery] GetGroupsQuery request)
    {
        return await Sender.Send(request);
    }
}
