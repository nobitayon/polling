namespace Delta.Polling.Services.CurrentUser;

public interface ICurrentUserService
{
    string? Username { get; }
    IEnumerable<string> RoleNames { get; }
}
