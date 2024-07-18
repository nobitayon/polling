namespace Delta.Polling.Infrastructure.Email.Microsoft;

public record MicrosoftEmailOptions
{
    public const string SectionKey = $"{nameof(Email)}:{nameof(Microsoft)}";

    public required string ApiUrl { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}
