namespace Delta.Polling.FrontEnd.Infrastructure.UserProfile.Another;

public record AnotherUserProfileOptions
{
    public const string SectionKey = $"{nameof(UserProfile)}:{nameof(SimpleTor)}";

    public required string ApiBaseUrl { get; init; }
    public required string TokenUrl { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
    public required string[] Scopes { get; init; } = ["SimpleTor.User.GetUser"];
}
