namespace Delta.Polling.WebAPI.Infrastructure.Authentication;

public record AuthenticationOptions
{
    public const string SectionKey = nameof(Authentication);

    public string Provider { get; set; } = AuthenticationProvider.SimpleTen;
}

public static class AuthenticationProvider
{
    public const string SimpleTen = nameof(SimpleTen);
}