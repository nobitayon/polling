using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Domain.Choices.Entities;
using Delta.Polling.Services.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class ChoiceMediaConfiguration : IEntityTypeConfiguration<ChoiceMedia>
{
    public void Configure(EntityTypeBuilder<ChoiceMedia> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.ChoiceMedias));
        builder.ConfigureFileProperties();

        _ = builder.HasOne(choiceMedia => choiceMedia.Choice)
            .WithMany(choice => choice.ChoiceMedias)
            .HasForeignKey(choiceMedia => choiceMedia.ChoiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
