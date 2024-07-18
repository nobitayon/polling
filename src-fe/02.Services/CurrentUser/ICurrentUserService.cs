namespace Delta.Polling.FrontEnd.Services.CurrentUser;

public interface ICurrentUserService
{
    string? Username { get; }
    IEnumerable<string> RoleNames { get; }
    string? AccessToken { get; }
}
