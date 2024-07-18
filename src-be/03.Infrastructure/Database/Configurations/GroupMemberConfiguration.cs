using Delta.Polling.Domain.Groups.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Delta.Polling.Infrastructure.Database.Extensions;

namespace Delta.Polling.Infrastructure.Database.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        _ = builder.ToTable("GroupMembers");
        builder.ConfigureCreatableProperties();
    }
}
