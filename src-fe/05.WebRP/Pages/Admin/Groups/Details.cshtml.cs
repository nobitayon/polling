namespace Delta.Polling.WebRP.Pages.Admin.Groups;

public class DetailsModel : PageModelBase
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    public void OnGet()
    {
    }
}
