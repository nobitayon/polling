using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Delta.Polling.WebRP.Pages.Account;

public class LoginModel : PageModel
{
    public IActionResult OnGet(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            returnUrl = Url.Content("~/");
        }

        if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl);
        }

        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = returnUrl
        };

        return Challenge(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme);
    }
}
