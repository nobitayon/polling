namespace Delta.Polling.WebAPI.Infrastructure.Documentation;

public record DocumentationOptions
{
    public const string SectionKey = nameof(Documentation);

    public required string Title { get; init; }
    public required string Version { get; init; }
}
