using Delta.Polling.Domain.Movies.Entities;
using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Infrastructure.Database.Statics;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.Movies));
        builder.ConfigureModifiableProperties();

        _ = builder.Property(e => e.ApprovedBy).HasMaxLength(DomainMaxLengthFor.Username);
        _ = builder.Property(e => e.Title).HasMaxLength(MoviesMaxLengthFor.Title);
        _ = builder.Property(e => e.Budget).HasColumnType(ColumnTypeFor.Money);
        _ = builder.Property(e => e.Storyline).HasMaxLength(MoviesMaxLengthFor.Storyline);
    }
}
