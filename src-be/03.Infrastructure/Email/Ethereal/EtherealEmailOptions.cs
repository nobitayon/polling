namespace Delta.Polling.Infrastructure.Email.Ethereal;

public record EtherealEmailOptions
{
    public const string SectionKey = $"{nameof(Email)}:{nameof(Ethereal)}";

    public required string Server { get; init; }
    public required int Port { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}
