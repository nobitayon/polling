namespace Delta.Polling.FrontEnd.Infrastructure.Authentication.Another;

public record AnotherAuthenticationOptions
{
    public const string SectionKey = $"{nameof(Authentication)}:{nameof(Another)}";

    public required string Authority { get; init; }
    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }

    public required string[] Scopes { get; init; } =
    [
       "Polling.API",
        "dataEventRecords"
    ];
}
