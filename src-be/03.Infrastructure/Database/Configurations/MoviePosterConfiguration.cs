using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class MoviePosterConfiguration : IEntityTypeConfiguration<MoviePoster>
{
    public void Configure(EntityTypeBuilder<MoviePoster> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.MoviePosters));
        builder.ConfigureFileProperties();

        _ = builder.HasOne(moviePoster => moviePoster.Movie)
            .WithMany(movie => movie.Posters)
            .HasForeignKey(moviePoster => moviePoster.MovieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
