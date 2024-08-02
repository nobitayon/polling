using Delta.Polling.Both.Admin.Groups.Queries.GetGroup;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.RemoveMember;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroup;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class DetailsModel(PagerService pagerService) : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    [BindProperty]
    public IEnumerable<MemberItem> MemberItems { get; set; } = default!;

    [BindProperty]
    public GroupItem GroupItem { get; set; } = default!;

    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int? p, int ps = 5)
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

        if (response.Problem != null)
        {
            Problem = response.Problem;
            return Page();
        }

        if (response.Result == null)
        {
            TempData["failed"] = "Error get group";
            return Page();
        }

        MemberItems = response.Result.Data.MemberItems.Items;
        GroupItem = response.Result.Data.GroupItem;

        Paging = pagerService.GetHtml("Groups/Details", response.Result.Data.MemberItems.TotalCount, query);

        return Page();
    }


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

        if (response.Problem is not null)
        {
            var viewDataLocal = new ViewDataDictionary(ViewData)
            {
                ["Data"] = null,
                ["PageSize"] = ps,
                ["Error"] = response.Problem.Detail
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

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success Remove Member";
            return RedirectToPage("/Admin/Groups/Details", new { groupId = GroupId });
        }
        else
        {
            TempData["failed"] = "Failed to remove member";
            return Page();
        }
    }
}
