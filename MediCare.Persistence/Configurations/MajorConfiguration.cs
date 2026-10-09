using MediCare.Domain.Majors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Persistence.Configurations;

public sealed class MajorConfiguration : IEntityTypeConfiguration<Major>
{
    public void Configure(EntityTypeBuilder<Major> builder)
    {
        builder.ToTable("Majors");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("MajorID")
            .HasConversion(id => id.Value, value => new MajorId(value))
            .ValueGeneratedNever();

        builder.Property(m => m.Name)
            .HasColumnName("MajorName")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Description)
            .HasColumnName("MajorDescription")
            .HasMaxLength(500);

        builder.HasIndex(m => m.Name).IsUnique();
        builder.Ignore(m => m.DomainEvents);
    }
}
