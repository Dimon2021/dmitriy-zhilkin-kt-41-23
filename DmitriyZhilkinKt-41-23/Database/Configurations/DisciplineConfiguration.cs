using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
    {
        public void Configure(EntityTypeBuilder<Discipline> builder)
        {
            builder.HasKey(d => d.DisciplineId);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        }
    }
}
