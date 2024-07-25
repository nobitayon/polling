namespace Delta.Polling.Infrastructure.Authentication.SimpleTen;

public record SimpleTenAuthenticationOptions
{
    public const string SectionKey = $"{nameof(Authentication)}:{nameof(SimpleTen)}";

    public required string AuthorityUrl { get; init; }
    public required string Audience { get; init; }
}
