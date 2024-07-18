using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Domain.Groups.Entities;
using Delta.Polling.Infrastructure.Database.Extensions;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        _ = builder.ToTable("Groups");
        builder.ConfigureCreatableProperties();
        _ = builder.Property(e => e.Name).HasMaxLength(GroupsMaxLengthFor.Name);
    }
}
