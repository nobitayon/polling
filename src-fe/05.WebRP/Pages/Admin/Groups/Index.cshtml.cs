using Delta.Polling.Both.Admin.Groups.Queries.GetGroups;
using Delta.Polling.Both.Common.Enums;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.AddGroup;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetGroups;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    [BindProperty]
    public IEnumerable<GroupItem> Groups { get; set; } = [];
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

    public async Task<IActionResult> OnGet(int? p, int? ps, string k, string kf, string sf, SortOrder? so)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetGroupsQuery
        {
            Page = page,
            PageSize = pageSize,
            SearchText = k,
            SearchField = kf,
            SortField = sf
        };

        if (so != null)
        {
            query.SortOrder = (SortOrder)so;
        }

        var response = await Sender.Send(query);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            return Page();
        }

        if (response.Result is not null)
        {
            Groups = response.Result.Data.Items;

            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSearchQuery(string querySearch)
    {
        var query = new GetGroupsQuery
        {
            Page = 1,
            PageSize = 5,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };

        var dictionaryParsing = ParseKeyValuePairs(querySearch);
        foreach (var kvp in dictionaryParsing)
        {
            if (kvp.Key == nameof(PaginatedListRequest.SortOrder))
            {
                var parsed = int.TryParse(kvp.Value, out var sortOrder);
                if (parsed)
                {
                    if (Enum.IsDefined(typeof(SortOrder), sortOrder))
                    {
                        query.SortOrder = (SortOrder)sortOrder;
                    }
                }
            }
            else if (kvp.Key == nameof(PaginatedListRequest.SortField))
            {
                query.SortField = kvp.Value;
            }
            else if (kvp.Key == nameof(PaginatedListRequest.Page))
            {
                var parsed = int.TryParse(kvp.Value, out var p);
                if (parsed)
                {
                    var page = PagerHelper.GetSafePage(p);
                    query.Page = page;
                }
            }
            else if (kvp.Key == nameof(PaginatedListRequest.PageSize))
            {
                var parsed = int.TryParse(kvp.Value, out var ps);
                if (parsed)
                {
                    var pageSize = PagerHelper.GetSafePageSize(ps);
                    query.PageSize = pageSize;
                }
            }
            else if (kvp.Key == nameof(PaginatedListRequest.SearchText))
            {
                query.SearchText = kvp.Value;
            }
            else if (kvp.Key == nameof(PaginatedListRequest.SearchField))
            {
                query.SearchField = kvp.Value;
            }
        }

        var response = await Sender.Send(query);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
        }

        if (response.Result is not null)
        {
            Groups = response.Result.Data.Items;

            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
        }

        return Page();
    }

    private Dictionary<string, string> ParseKeyValuePairs(string input)
    {
        var result = new Dictionary<string, string>();

        var pairs = input.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var pair in pairs)
        {
            var keyValue = pair.Split([':'], 2);
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim();
                var value = keyValue[1].Trim();
                result[key] = value;
            }
        }

        return result;
    }
}
