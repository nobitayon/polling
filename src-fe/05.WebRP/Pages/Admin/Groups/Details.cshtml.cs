using Delta.Polling.Both.Admin.Groups.Queries.GetGroup;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.RemoveMember;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroup;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    [BindProperty]
    public GroupItem? GroupItem { get; set; }


    public async Task<IActionResult> OnGetMemberDataTable(int? p, int ps = 5)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetGroupQuery
        {
            GroupId = GroupId,
            Page = page,
            PageSize = pageSize,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };

        var response = await Sender.Send(query);

        if (response.Error is not null)
        {
            var viewDataLocal = new ViewDataDictionary(ViewData)
            {
                ["Data"] = null,
                ["PageSize"] = ps,
                ["Error"] = response.Error.Detail
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Admin/Groups/PartialCustom/_TableMember.cshtml",
                ViewData = viewDataLocal
            };
        }

        if (response.Result is null)
        {
            var viewDataLocal = new ViewDataDictionary(ViewData)
            {
                ["Data"] = null,
                ["PageSize"] = ps,
                ["Error"] = "Error get groups"
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Admin/Groups/PartialCustom/_TableMember.cshtml",
                ViewData = viewDataLocal
            };
        }

        var result = response.Result.Data;

        GroupItem = result.GroupItem;

        var viewData = new ViewDataDictionary(ViewData)
        {
            ["Data"] = result,
            ["PageSize"] = ps,
            ["Error"] = null
        };

        TempData["Title"] = result.GroupItem.Name;

        return new PartialViewResult
        {
            ViewName = "~/Pages/Admin/Groups/PartialCustom/_TableMember.cshtml",
            ViewData = viewData
        };
    }

    public async Task<IActionResult> OnPostRemoveMember(RemoveMemberCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Error is not null)
        {
            Error = response.Error;
            Console.WriteLine("Error");
            return Page();
        }

        if (response.Result is not null)
        {
            Console.WriteLine("sukses");
            TempData["success"] = "Success Remove Member";
            return RedirectToPage("/Admin/Groups/Details", new { groupId = GroupId });
        }
        else
        {
            Console.WriteLine("failed");
            TempData["failed"] = "Failed to remove member";
            return Page();
        }
    }
}
