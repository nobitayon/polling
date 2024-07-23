namespace Delta.Polling.Services.CurrentUser;

public interface ICurrentUserService
{
    string? Username { get; }
    string? AccessToken { get; }
}
