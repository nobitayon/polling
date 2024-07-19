using Delta.Polling.Both.Common.Enums;
using Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;
using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;
using Delta.Polling.Logics.Member.Groups.Queries.GetMyGroup;
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

    // TODO: apa harus ditulis satu-satu
    [HttpGet("{groupId:guid}")]
    public async Task<GetMyGroupOutput> GetMyGroup(
        [FromRoute] Guid groupId,
        [FromQuery] string? searchText,
        [FromQuery] SortOrder? sortOrder,
        [FromQuery] string? sortField,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var request = new GetMyGroupQuery { GroupId = groupId, Page = page, PageSize = pageSize };

        if (searchText != null)
        {
            request.SearchText = searchText;
        }

        if (sortOrder != null)
        {
            request.SortOrder = sortOrder;
        }

        if (sortField != null)
        {
            request.SortField = sortField;
        }

        return await Sender.Send(request);
    }
}

