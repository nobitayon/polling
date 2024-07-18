namespace Delta.Polling.FrontEnd.Infrastructure.BackEnd;

public record BackEndOptions
{
    public const string SectionKey = nameof(BackEnd);

    public required string ApiBaseUrl { get; init; }
}
