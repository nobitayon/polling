namespace Delta.Polling.Infrastructure.Email;

public record EmailOptions
{
    public const string SectionKey = nameof(Email);

    public required string Provider { get; init; }
}

public static class EmailProvider
{
    public const string Google = nameof(Google);
    public const string Microsoft = nameof(Microsoft);
}
