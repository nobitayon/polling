using Delta.Polling.Services.Email;

namespace Delta.Polling.Infrastructure.Email;

public record EmailOptions
{
    public const string SectionKey = nameof(Email);

    public required string Provider { get; init; }
    public required MailBoxModel From { get; init; }
    public required string LinkBaseUrl { get; init; }
}

public static class EmailProvider
{
    public const string Ethereal = nameof(Ethereal);
    public const string Dummy = nameof(Dummy);
}
