using Delta.Polling.Both.Member.Groups.Queries.GetMyGroups;
using Delta.Polling.FrontEnd.Logics.Member.Groups.Queries.GetMyGroups;

namespace Delta.Polling.WebRP.Pages.Member.Groups;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    public IEnumerable<GroupItem> Groups { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGet(int? p, int? ps)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyGroupsQuery
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
            Error = response.Error;

            return Page();
        }

        if (response.Result is not null)
        {
            Groups = response.Result.Data.Items.ToList();

            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
        }

        return Page();
    }
}
