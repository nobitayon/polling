namespace Delta.Polling.WebRP.Infrastructure.Authentication;

public record AuthenticationOptions
{
    public const string SectionKey = $"{nameof(Authentication)}";

    public required string Provider { get; init; } = AuthenticationProvider.SimpleTen;
}

public static class AuthenticationProvider
{
    public const string SimpleTen = nameof(SimpleTen);
}
