using Delta.Polling.Domain.Voters.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class VoterConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        _ = builder.ToTable("Voters");
        builder.ConfigureCreatableProperties();
    }
}
