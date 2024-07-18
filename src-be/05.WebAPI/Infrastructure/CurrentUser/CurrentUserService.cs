using Delta.Polling.Services.CurrentUser;
using System.Security.Claims;

namespace Delta.Polling.WebAPI.Infrastructure.CurrentUser;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public string? Username
    {
        get
        {
            if (httpContextAccessor.HttpContext is null)
            {
                throw new Exception("HttpContext is null");
            }

            var user = httpContextAccessor.HttpContext.User;
            var identity = user.Identity as ClaimsIdentity
                ?? throw new Exception("Identity is null");

            return identity.Name;
        }
    }

    public IEnumerable<string> RoleNames
    {
        get
        {
            if (httpContextAccessor.HttpContext is null)
            {
                throw new Exception("HttpContext is null");
            }

            var user = httpContextAccessor.HttpContext.User;
            var identity = user.Identity as ClaimsIdentity
                ?? throw new Exception("Identity is null");

            var roleNames = new List<string>();

            foreach (var claim in identity.Claims)
            {
                if (claim.Type == ClaimTypes.Role)
                {
                    roleNames.Add(claim.Value);
                }
            }

            return roleNames;
        }
    }
}
