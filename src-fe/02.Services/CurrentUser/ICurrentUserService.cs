namespace Delta.Polling.FrontEnd.Services.CurrentUser;

public interface ICurrentUserService
{
    string? Username { get; }
    string? AccessToken { get; }
    IEnumerable<string> RoleNames { get; }
}
