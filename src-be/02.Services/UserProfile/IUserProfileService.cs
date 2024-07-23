namespace Delta.Polling.Services.UserProfile;

public interface IUserProfileService
{
    Task<UserProfileItem> GetMyProfileAsync(string jwt, CancellationToken cancellationToken = default);
    Task<UserProfileItem?> GetUserProfileAsync(string username, CancellationToken cancellationToken = default);
}

public record UserProfileItem
{
    public required string Email { get; init; }
    public required string Name { get; init; }
}
