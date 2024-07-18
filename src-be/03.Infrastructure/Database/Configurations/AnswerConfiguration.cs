using Delta.Polling.Domain.Answers.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.Answers));
        builder.ConfigureCreatableProperties();

        // many-to-many relationship between Voter dan Choice
        _ = builder.HasOne(e => e.Voter)
           .WithMany(e => e.Answers)
           .HasForeignKey(e => e.VoterId)
           .OnDelete(DeleteBehavior.Restrict);

        _ = builder.HasOne(e => e.Choice)
            .WithMany(e => e.Answers)
            .HasForeignKey(e => e.ChoiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
