using Delta.Polling.FrontEnd.Infrastructure.BackEnd;
using Delta.Polling.FrontEnd.Services.CurrentUser;
using Microsoft.Extensions.Options;

namespace Delta.Polling.WebRP.Pages.Member.Polls;

public class SomethingModel(
    ICurrentUserService currentUserService,
    IOptions<BackEndOptions> backEndOptions) : PageModel
{
    public IActionResult OnGetAccessToken()
    {
        if (currentUserService.AccessToken == null)
        {
            return new JsonResult(new { isValid = false });
        }

        return new JsonResult(new { isValid = true, accessToken = currentUserService.AccessToken, apiBaseUri = backEndOptions.Value.ApiBaseUrl });
    }
}
