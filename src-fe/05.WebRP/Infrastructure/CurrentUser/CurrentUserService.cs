using System.Security.Claims;
using Delta.Polling.FrontEnd.Services.CurrentUser;

namespace Delta.Polling.WebRP.Infrastructure.CurrentUser;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly ClaimsPrincipal _claimsPrincipal = httpContextAccessor.HttpContext!.User;

    public string? Username => _claimsPrincipal.FindFirstValue(KnownClaimTypes.PreferredUsername);
    public string? AccessToken => _claimsPrincipal.FindFirstValue(CustomClaimTypes.AccessToken);

    public IEnumerable<string> RoleNames => _claimsPrincipal.Claims
        .Where(claim => claim.Type == ClaimTypes.Role)
        .Select(claim => claim.Value);
}
