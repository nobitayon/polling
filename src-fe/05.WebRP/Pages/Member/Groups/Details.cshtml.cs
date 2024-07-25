namespace Delta.Polling.WebRP.Pages.Member.Groups;

public class DetailsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid GroupId { get; init; }

    public void OnGet()
    {

    }
}
