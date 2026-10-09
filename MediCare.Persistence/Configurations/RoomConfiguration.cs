using MediCare.Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Persistence.Configurations
{
    public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Rooms");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasColumnName("RoomID")
                .HasConversion(id => id.Value, value => new RoomId(value))
                .ValueGeneratedNever();

            builder.Property(r => r.Name)
                .HasColumnName("RoomName")
                .IsRequired();

            builder.HasIndex(r => r.Name).IsUnique();

            builder.Property(r => r.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(r => r.IsActive).IsRequired();

            builder.Ignore(r => r.DomainEvents);
        }
    }
}
