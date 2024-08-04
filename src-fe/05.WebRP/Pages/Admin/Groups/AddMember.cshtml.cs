using Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;
using Delta.Polling.Both.Common.Enums;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Commands.AddMember;
using Delta.Polling.FrontEnd.Logics.Admin.Groups.Queries.GetUsersNotMemberFromGroup;

namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class AddMemberModel(PagerService pagerService) : PageModelBase
{
    [BindProperty]
    public IEnumerable<MemberItem> MemberItems { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    public string Paging { get; set; } = string.Empty;

    //public async Task<IActionResult> OnGetAsync(int? p, int ps = 5)
    //{
    //    //var page = PagerHelper.GetSafePage(p);
    //    //var pageSize = PagerHelper.GetSafePageSize(ps);

    //    //var query = new GetUsersNotMemberFromGroupQuery
    //    //{
    //    //    GroupId = GroupId,
    //    //    Page = page,
    //    //    PageSize = pageSize,
    //    //    SearchText = null,
    //    //    SortField = null,
    //    //    SortOrder = null
    //    //};

    //    //var response = await Sender.Send(query);

    //    //if (response.Error != null)
    //    //{
    //    //    Error = response.Error;
    //    //    return Page();
    //    //}

    //    //if (response.Result == null)
    //    //{
    //    //    TempData["failed"] = "Error get group";
    //    //    return Page();
    //    //}

    //    //MemberItems = response.Result.Data.Items;
    //    await LoadData(p, ps);

    //    return Page();
    //}

    private async Task LoadData(GetUsersNotMemberFromGroupQuery query)
    {
        var response = await Sender.Send(query);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            return;
        }

        if (response.Result is not null)
        {
            MemberItems = response.Result.Data.Items;

            Paging = pagerService.GetHtml($"", response.Result.Data.TotalCount, query);
        }
    }

    public async Task<IActionResult> OnPostAddMember(AddMemberCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Problem is not null)
        {
            Problem = response.Problem;
            Notifier.Error($"Error add member {command.Username} to group {command.GroupId}");
            return Page();
        }

        TempData["success"] = "Success Add Member";
        Notifier.Success($"Success add member {command.Username} to group {command.GroupId}");
        await LoadData(new GetUsersNotMemberFromGroupQuery
        {
            GroupId = GroupId,
            Page = 1,
            PageSize = 5,
            SearchText = null,
            SearchField = null,
            SortField = null
        });
        return Page();
    }

    public async Task<IActionResult> OnGet(int? p, int? ps, string k, string kf, string sf, SortOrder? so)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetUsersNotMemberFromGroupQuery
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

        await LoadData(query);

        return Page();
    }

    public async Task<IActionResult> OnPostSearchQuery(string querySearch)
    {
        var query = new GetUsersNotMemberFromGroupQuery
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

        await LoadData(query);

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
