using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        private const string TableName = "Specialtys";

        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.SpecialtyId)
                .HasName($"pk_{TableName}_specialty_id");

            builder.Property(p => p.SpecialtyId)
                .HasColumnName("specialty_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title");

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("code");
        }
    }
}
