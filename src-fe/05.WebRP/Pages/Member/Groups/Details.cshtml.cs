using Delta.Polling.Both.Common.Enums;
using Delta.Polling.Both.Member.Groups.Queries.GetMyGroup;
using Delta.Polling.FrontEnd.Logics.Member.Groups.Queries.GetMyGroup;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Commands.AddPoll;

namespace Delta.Polling.WebRP.Pages.Member.Groups;

public class DetailsModel(PagerService pagerService) : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    public GroupItem GroupItem { get; set; } = default!;

    public string Paging { get; set; } = string.Empty;

    [BindProperty]
    public AddPollCommand Input { get; set; } = default!;

    public async Task<IActionResult> OnGet(int? p, int? ps, string k, string kf, string sf, SortOrder? so)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyGroupQuery
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

        if (response.Result is null)
        {
            TempData["failed"] = "failed get poll";

            return Page();
        }

        GroupItem = response.Result.Data;
        Paging = pagerService.GetHtml("", response.Result.Data.PollItems.TotalCount, query);

        return Page();
    }

    public async Task<IActionResult> OnPostSearchQuery(string querySearch)
    {
        var query = new GetMyGroupQuery
        {
            GroupId = GroupId,
            Page = 1,
            PageSize = 5,
            SearchField = null,
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
            GroupItem = response.Result.Data;

            Paging = pagerService.GetHtml("", response.Result.Data.PollItems.TotalCount, query);
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

    public async Task<IActionResult> OnPostAddPollAsync()
    {
        var response = await Sender.Send(Input);

        if (response.Problem is not null)
        {
            Problem = response.Problem;

            return Page();
        }

        if (response.Result is not null)
        {
            TempData["success"] = "Success create poll";

            return RedirectToPage("/Member/Polls/Details", new { pollId = response.Result.Data.PollId });
        }
        else
        {

            TempData["failed"] = "Failed to create poll";

            return Page();
        }
    }
}
