using System.Text;
using Delta.Polling.Base.Polls.Enums;
using Delta.Polling.Both.Common.Enums;
using Delta.Polling.Both.Member.Polls.Queries.GetMyPolls;
using Delta.Polling.FrontEnd.Logics.Member.Polls.Queries.GetMyPolls;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class MyModel(PagerService pagerService) : PageModelBase
{
    public IEnumerable<PollItemModel> Polls { get; set; } = [];
    public string Paging { get; set; } = string.Empty;

    [BindProperty]
    public string? QuerySearch { get; set; } = "";

    public async Task<IActionResult> OnGet(int? p, int? ps, string k, string kf, string sf, SortOrder? so)
    {
        QuerySearch = BuildQuery(p, ps, k, kf, sf, so);

        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetMyPollsQuery
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

        if (response.Result is not null)
        {
            Polls = response.Result.Data.Items.Select(item => new PollItemModel
            {
                Id = item.Id,
                Status = item.Status,
                Title = item.Title
            }).ToList();

            Paging = pagerService.GetHtml("", response.Result.Data.TotalCount, query);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSearchQuery()
    {
        var query = new GetMyPollsQuery
        {
            Page = 1,
            PageSize = 5,
            SearchText = null,
            SortField = null,
            SortOrder = null
        };
        if (!string.IsNullOrEmpty(QuerySearch))
        {
            var dictionaryParsing = ParseKeyValuePairs(QuerySearch);
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
        }

        var response = await Sender.Send(query);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
        }

        if (response.Result is not null)
        {
            Polls = response.Result.Data.Items.Select(item => new PollItemModel
            {
                Id = item.Id,
                Status = item.Status,
                Title = item.Title
            }).ToList();

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

    private string BuildQuery(int? p, int? ps, string k, string kf, string sf, SortOrder? so)
    {
        if (p == null && ps == null && k == null && kf == null && sf == null && so == null)
        {
            return "";
        }

        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(kf))
        {
            _ = sb.Append($"SearchField:{kf};");
        }

        if (!string.IsNullOrEmpty(k))
        {
            _ = sb.Append($"SearchText:{k};");
        }

        if (!string.IsNullOrEmpty(sf))
        {
            _ = sb.Append($"SortField:{sf};");
        }

        if (so != null)
        {
            _ = sb.Append($"SortOrder:{(int)so};");
        }

        if (p != null)
        {
            _ = sb.Append($"Page:{p};");
        }

        if (ps != null)
        {
            _ = sb.Append($"PageSize:{ps};");
        }

        var buildedString = sb.ToString().TrimEnd(';');

        return buildedString;
    }
}

public record PollItemModel : PollItem
{
    public string StatusColor
    {
        get
        {
            return Status switch
            {
                PollStatus.Draft => "bg-secondary",
                PollStatus.Ongoing => "bg-warning",
                PollStatus.Finished => "bg-success",
                _ => "bg-primary",
            };
        }
    }
}
