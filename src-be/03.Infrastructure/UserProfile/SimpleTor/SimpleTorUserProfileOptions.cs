namespace Delta.Polling.Infrastructure.UserProfile.SimpleTor;

public record SimpleTorUserProfileOptions
{
    public const string SectionKey = $"{nameof(UserProfile)}:{nameof(SimpleTor)}";

    public required string ApiBaseUrl { get; init; }
    public required string TokenUrl { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string[] Scopes { get; init; } =
    [
        "SimpleTor.User.GetUser",
        "SimpleTor.User.GetUsers",
        "SimpleTor.Application.GetUsers",
        "SimpleTor.Application.GetRoleUsers"
    ];
    public required string ApplicationId { get; init; }
}
