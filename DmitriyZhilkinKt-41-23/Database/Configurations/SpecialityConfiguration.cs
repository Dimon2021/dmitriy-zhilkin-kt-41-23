using DmitriyZhilkinKt_41_23.Database.Configurations;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    

    public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.HasKey(s => s.SpecialityId);
            builder.Property(s => s.Title).IsRequired().HasMaxLength(200);
            builder.Property(s => s.Code).IsRequired().HasMaxLength(20);
        }
    }
}
