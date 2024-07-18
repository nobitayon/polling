using Delta.Polling.Domain.Voters.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class VoterConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.Voters));
        builder.ConfigureCreatableProperties();

        _ = builder.HasOne(e => e.Poll)
            .WithMany(e => e.Voters)
            .HasForeignKey(e => e.PollId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
