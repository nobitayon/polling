using Delta.Polling.Domain.Polls.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;

namespace Delta.Polling.Infrastructure.Database.Configurations;
public class PollConfiguration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        _ = builder.ToTable("Polls");
        builder.ConfigureModifiableProperties();
        _ = builder.Property(e => e.Title).HasMaxLength(PollsMaxLengthFor.Title);
        _ = builder.Property(e => e.Question).HasMaxLength(PollsMaxLengthFor.Question);

        // one-to-many relationship between Group and Poll
        _ = builder.HasOne(e => e.Group)
            .WithMany(e => e.Polls)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
