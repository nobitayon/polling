using Delta.Polling.Both.Common.Enums;
using Delta.Polling.Both.Member.Votes.Queries.GetMyVotes;
using Delta.Polling.FrontEnd.Logics.Member.Votes.Queries.GetMyVotes;

namespace Delta.Polling.WebRP.Pages.Member.Votes;

public class IndexModel(PagerService pagerService) : PageModelBase
{
    [BindProperty]
    public IEnumerable<VoteItem> VoteItems { get; set; } = default!;

    public string Paging { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int? p, int? ps, string? k, string? kf, string? sf, SortOrder? so)
    {
        await LoadData(p, ps, k, kf, sf, so);

        return Page();
    }

    public async Task<IActionResult> OnPostSearchQuery(string querySearch)
    {
        var query = new GetMyVotesQuery
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

        await LoadData(query.Page, query.PageSize, query.SearchText, query.SearchField, query.SortField, query.SortOrder);

        return Page();
    }

    private async Task LoadData(int? p, int? ps, string? k, string? kf, string? sf, SortOrder? so)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyVotesQuery
        {
            Page = page,
            PageSize = pageSize,
            SearchText = k,
            SearchField = kf,
            SortField = sf,
            SortOrder = so
        };

        var response = await Sender.Send(query);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            return;
        }

        if (response.Result is not null)
        {
            VoteItems = response.Result.Data.Items;
            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
            return;
        }
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
