namespace Delta.Polling.Infrastructure.UserProfile;

public record UserProfileOptions
{
    public const string SectionKey = $"{nameof(UserProfile)}";

    public required string Provider { get; init; } = UserProfileProvider.SimpleTor;
}

public static class UserProfileProvider
{
    public const string SimpleTor = nameof(SimpleTor);
}
