namespace Delta.Polling.FrontEnd.Services.UserProfile;

public interface IUserProfileService
{
    Task<UserProfileItem> GetUserProfileAsync(string username, CancellationToken cancellationToken = default);
}

public record UserProfileItem
{
    public required string Email { get; init; }
    public required string Name { get; init; }
}
