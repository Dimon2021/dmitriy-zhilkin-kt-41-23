using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasKey(g => g.GradeId);

            builder.HasOne(g => g.Student)
                   .WithMany(s => s.Grades)
                   .HasForeignKey(g => g.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.Discipline)
                   .WithMany(d => d.Grades)
                   .HasForeignKey(g => g.DisciplineId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
