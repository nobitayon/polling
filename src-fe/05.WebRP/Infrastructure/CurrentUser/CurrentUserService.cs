using System.Security.Claims;
using Delta.Polling.FrontEnd.Services.CurrentUser;
using Delta.Polling.FrontEnd.Services.CurrentUser.Statics;

namespace Delta.Polling.WebRP.Infrastructure.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsIdentity _claimsIdentity;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            throw new Exception("HttpContext is null");
        }

        _claimsIdentity = httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity
            ?? throw new Exception("Identity is null");
    }

    public string? Username => _claimsIdentity.Name;

    public IEnumerable<string> RoleNames => _claimsIdentity.Claims
        .Where(claim => { return claim.Type == ClaimTypes.Role; })
        .Select(claim => { return claim.Value; });

    public string? AccessToken => _claimsIdentity.FindFirst(ClaimTypeFor.AccessToken)?.Value;
}
