﻿using System.Security.Claims;
using Delta.Polling.Services.CurrentUser;
using Microsoft.AspNetCore.Http;

namespace Delta.Polling.Infrastructure.CurrentUser;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly ClaimsPrincipal _claimsPrincipal = httpContextAccessor.HttpContext!.User;

    public string? Username => _claimsPrincipal.FindFirstValue(KnownClaimTypes.PreferredUsername);
    public string? AccessToken => _claimsPrincipal.FindFirstValue(CustomClaimTypes.AccessToken);
}
