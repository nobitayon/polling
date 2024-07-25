using Delta.Polling.Domain.Choices.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
{
    public void Configure(EntityTypeBuilder<Choice> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.Choices));
        builder.ConfigureModifiableProperties();
        _ = builder.Property(e => e.Description).HasMaxLength(ChoicesMaxLengthFor.Description);

        _ = builder.HasOne(e => e.Poll)
            .WithMany(e => e.Choices)
            .HasForeignKey(e => e.PollId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
