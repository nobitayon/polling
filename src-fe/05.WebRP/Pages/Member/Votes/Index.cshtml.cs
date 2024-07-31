using Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;
using Delta.Polling.FrontEnd.Logics.Member.Votes.Queries.GetMyVotes;

namespace Delta.Polling.WebRP.Pages.Member.Votes;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    [BindProperty]
    public IEnumerable<VoteItem> VoteItems { get; set; } = default!;

    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int? p, int ps = 5)
    {
        await LoadData(p, ps);

        return Page();
    }

    private async Task LoadData(int? p, int ps = 5)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyVotesQuery
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
            return;
        }

        if (response.Result is not null)
        {
            VoteItems = response.Result.Data.Items;

            Paging = pagerService.GetHtml("Votes/Index", response.Result.Data.TotalCount, query);
        }
    }
}
