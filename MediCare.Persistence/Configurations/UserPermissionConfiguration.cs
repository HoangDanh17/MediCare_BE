using MediCare.Domain.Permissions;
using MediCare.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Persistence.Configurations;

public sealed class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable("User_Permission");

        builder.HasKey(up => new { up.UserId, up.PermissionId });

        builder.Property(up => up.UserId)
            .HasColumnName("UserID")
            .HasConversion(id => id.Value, value => new UserId(value));

        builder.Property(up => up.PermissionId)
            .HasColumnName("PermissionID")
            .HasConversion(id => id.Value, value => new PermissionId(value));

        builder.Property(up => up.IsGranted)
            .HasColumnName("IsGranted")
            .IsRequired();

        builder.HasOne<Permission>()
            .WithMany()
            .HasForeignKey(up => up.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(up => up.PermissionId);
    }
}