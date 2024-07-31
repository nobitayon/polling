using Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.AddGroup;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroups;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class IndexModel : PageModelBase
{
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnGetDataTable(int? p, int ps = 5)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetGroupsQuery
        {
            Page = page,
            PageSize = pageSize,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };

        var response = await Sender.Send(query);

        if (response.Problem is not null)
        {
            var viewDataLocal = new ViewDataDictionary(ViewData)
            {
                ["Data"] = null,
                ["Count"] = 0,
                ["PageSize"] = ps,
                ["Error"] = response.Problem.Detail
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Admin/Groups/PartialCustom/_TableGroup.cshtml",
                ViewData = viewDataLocal
            };
        }

        if (response.Result is null)
        {
            var viewDataLocal = new ViewDataDictionary(ViewData)
            {
                ["Data"] = null,
                ["Count"] = 0,
                ["PageSize"] = ps,
                ["Error"] = "Error get groups"
            };

            return new PartialViewResult
            {
                ViewName = "~/Pages/Admin/Groups/PartialCustom/_TableGroup.cshtml",
                ViewData = viewDataLocal
            };
        }

        var groups = response.Result.Data.Items;
        var totalCount = response.Result.Data.TotalCount;

        var viewData = new ViewDataDictionary(ViewData)
        {
            ["Data"] = groups,
            ["Count"] = totalCount,
            ["PageSize"] = ps,
            ["Error"] = null
        };

        return new PartialViewResult
        {
            ViewName = "~/Pages/Admin/Groups/PartialCustom/_TableGroup.cshtml",
            ViewData = viewData
        };
    }

    public IActionResult OnGetCreateGroup()
    {
        return new PartialViewResult
        {
            ViewName = "~/Pages/Admin/Groups/PartialCustom/_CreateGroupModal.cshtml"
        };
    }

    public async Task<IActionResult> OnPostCreateGroup(AddGroupCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            TempData["failed"] = Problem.Detail;

            var redirectUrlLocal = Url.Page("/Admin/Groups");

            return new JsonResult(new { isValid = false, redirectUrl = redirectUrlLocal });
        }

        if (response.Result is null)
        {
            TempData["failed"] = "Failed to create group";

            var redirectUrlLocal = Url.Page("/Admin/Groups");

            return new JsonResult(new { isValid = true, redirectUrl = redirectUrlLocal });
        }

        TempData["success"] = "Success create group";

        var redirectUrl = Url.Page("/Admin/Groups/Details", new { groupId = response.Result.Data.GroupId });

        return new JsonResult(new { isValid = true, redirectUrl = redirectUrl });
    }
}
