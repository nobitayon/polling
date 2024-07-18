using Delta.Polling.Domain.Groups.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;
using Delta.Polling.Services.Database;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        _ = builder.ToTable(nameof(IDatabaseService.GroupMembers));
        builder.ConfigureCreatableProperties();

        _ = builder.HasOne(e => e.Group)
            .WithMany(e => e.GroupMembers)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
