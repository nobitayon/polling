namespace Delta.Polling.Infrastructure.Database;

public record DatabaseOptions
{
    public const string SectionKey = nameof(Database);

    public required string ConnectionString { get; init; }
}
