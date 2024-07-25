namespace Delta.Polling.Domain.Movies.Entities;

public record MoviePoster : FileEntity
{
    public required Guid MovieId { get; init; }
    public Movie Movie { get; init; } = default!;

    public required string Description { get; set; }
}
