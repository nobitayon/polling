using Delta.Polling.Both.Admin.Groups.Commands.AddGroup;
using Delta.Polling.Both.Admin.Groups.Commands.AddMember;
using Delta.Polling.Both.Admin.Groups.Queries.GetGroups;
using Delta.Polling.Logics.Admin.Groups.Commands.AddGroup;
using Delta.Polling.Logics.Admin.Groups.Commands.AddMember;
using Delta.Polling.Logics.Admin.Groups.Commands.RemoveMember;
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

    [HttpPost]
    public async Task<AddGroupOutput> AddGroup([FromForm] AddGroupCommand request)
    {
        return await Sender.Send(request);
    }

    [HttpPost("{groupId:guid}/add-member")]
    public async Task<AddMemberOutput> AddMember([FromRoute] Guid groupId, [FromForm] AddMemberCommand request)
    {
        if (groupId != request.GroupId)
        {
            throw new MismatchException(nameof(request.GroupId), groupId, request.GroupId);
        }

        return await Sender.Send(request);
    }

    [HttpPost("{groupId:guid}/remove-member")]
    public async Task RemoveMember([FromRoute] Guid groupId, [FromForm] RemoveMemberCommand request)
    {
        if (groupId != request.GroupId)
        {
            throw new MismatchException(nameof(request.GroupId), groupId, request.GroupId);
        }

        await Sender.Send(request);
    }
}
