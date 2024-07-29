namespace Delta.Polling.WebRP.Pages;

public class IndexModel : PageModelBase
{
    public IActionResult OnGet()
    {
        if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
        {
            if (HttpContext.User.IsInRole(RoleNameFor.Member))
            {
                return LocalRedirect("/Member/Index");
            }
            else if (HttpContext.User.IsInRole(RoleNameFor.Administrator))
            {
                return LocalRedirect("/Admin/Index");
            }
        }
        return Page();
    }
}
