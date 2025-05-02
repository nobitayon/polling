namespace Delta.Polling.FrontEnd.Services.UserProfile;

public interface IUserProfileService
{
    public Task<UserProfileItem> GetMyProfileAsync(string jwt, CancellationToken cancellationToken = default);
}

public record UserProfileItem
{
    public required string Email { get; init; }
    public required string Name { get; init; }
}
