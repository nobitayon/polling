namespace Delta.Polling.FrontEnd.Services.CurrentUser;

public interface ICurrentUserService
{
    public string? Username { get; }
    public string? AccessToken { get; }
    public IEnumerable<string> RoleNames { get; }
}
