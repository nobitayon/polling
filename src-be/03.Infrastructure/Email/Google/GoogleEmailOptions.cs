namespace Delta.Polling.Infrastructure.Email.Google;

public record GoogleEmailOptions
{
    public const string SectionKey = $"{nameof(Email)}:{nameof(Google)}";

    public required string Server { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}

