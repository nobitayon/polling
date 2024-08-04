using Delta.Polling.Both.Admin.Groups.Queries.GetGroup;
using Delta.Polling.Both.Common.Enums;
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

    //public async Task<IActionResult> OnGetAsync(int? p, int ps = 5)
    //{
    //    var page = PagerHelper.GetSafePage(p);
    //    var pageSize = PagerHelper.GetSafePageSize(ps);

    //    var query = new GetGroupQuery
    //    {
    //        GroupId = GroupId,
    //        Page = page,
    //        PageSize = pageSize,
    //        SearchText = null,
    //        SortField = null,
    //        SortOrder = null
    //    };

    //    var response = await Sender.Send(query);

    //    if (response.Problem != null)
    //    {
    //        Problem = response.Problem;
    //        return Page();
    //    }

    //    if (response.Result == null)
    //    {
    //        TempData["failed"] = "Error get group";
    //        return Page();
    //    }

    //    MemberItems = response.Result.Data.MemberItems.Items;
    //    GroupItem = response.Result.Data.GroupItem;

    //    Paging = pagerService.GetHtml("Groups/Details", response.Result.Data.MemberItems.TotalCount, query);

    //    return Page();
    //}

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

    public async Task<IActionResult> OnGet(int? p, int? ps, string k, string kf, string sf, SortOrder? so)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetGroupQuery
        {
            GroupId = GroupId,
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
            GroupItem = response.Result.Data.GroupItem;

            MemberItems = response.Result.Data.MemberItems.Items;

            Paging = pagerService.GetHtml("", response.Result.Data.MemberItems.TotalCount, query);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSearchQuery(string querySearch)
    {
        var query = new GetGroupQuery
        {
            GroupId = GroupId,
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
            GroupItem = response.Result.Data.GroupItem;

            MemberItems = response.Result.Data.MemberItems.Items;

            Paging = pagerService.GetHtml("", response.Result.Data.MemberItems.TotalCount, query);
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
