using Delta.Polling.Both.Admin.Groups.Queries.GetUsersNotMemberFromGroup;
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

    public async Task<IActionResult> OnGetAsync(int? p, int ps = 5)
    {
        //var page = PagerHelper.GetSafePage(p);
        //var pageSize = PagerHelper.GetSafePageSize(ps);

        //var query = new GetUsersNotMemberFromGroupQuery
        //{
        //    GroupId = GroupId,
        //    Page = page,
        //    PageSize = pageSize,
        //    SearchText = null,
        //    SortField = null,
        //    SortOrder = null
        //};

        //var response = await Sender.Send(query);

        //if (response.Error != null)
        //{
        //    Error = response.Error;
        //    return Page();
        //}

        //if (response.Result == null)
        //{
        //    TempData["failed"] = "Error get group";
        //    return Page();
        //}

        //MemberItems = response.Result.Data.Items;
        await LoadData(p, ps);

        return Page();
    }

    private async Task LoadData(int? p, int ps = 5)
    {
        var page = PagerHelper.GetSafePage(p);
        var pageSize = PagerHelper.GetSafePageSize(ps);

        var query = new GetUsersNotMemberFromGroupQuery
        {
            GroupId = GroupId,
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
            MemberItems = response.Result.Data.Items;

            Paging = pagerService.GetHtml($"/Admin/Groups/AddMember/{GroupId}", response.Result.Data.TotalCount, query);
        }
    }

    public async Task<IActionResult> OnPostAddMember(AddMemberCommand command)
    {
        var response = await Sender.Send(command);

        if (response.Error is not null)
        {
            Error = response.Error;
            return Page();
        }

        TempData["success"] = "Success Add Member";
        await LoadData(1);
        return Page();
    }
}
