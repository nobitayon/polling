namespace Delta.Polling.Services.CurrentUser;

public interface ICurrentUserService
{
    public string? Username { get; }
    public string? AccessToken { get; }
}
