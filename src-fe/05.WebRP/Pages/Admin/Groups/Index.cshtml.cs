using Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroups;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class IndexModel : PageModelBase
{
    public void OnGet()
    {
    }

    public string Paging { get; set; } = string.Empty;

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

        if (response.Error is not null)
        {
            var viewDataLocal = new ViewDataDictionary(ViewData)
            {
                ["Data"] = null,
                ["Count"] = 0,
                ["PageSize"] = ps,
                ["Error"] = response.Error.Detail
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
}
