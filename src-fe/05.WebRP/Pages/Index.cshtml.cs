namespace Delta.Polling.WebRP.Pages;

public class IndexModel : PageModelBase
{
    public IActionResult OnGet()
    {
        if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
        {
            return LocalRedirect("/Member/Index");
        }

        return Page();
    }
}
