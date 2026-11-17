using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        private const string TableName = "Grades";

        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.GradeId)
                .HasName($"pk_{TableName}_grade_id");

            builder.Property(p => p.GradeId)
                .HasColumnName("grade_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("value");

            builder.Property(p => p.StudentId)
                .IsRequired()
                .HasColumnName("student_id");

            builder.Property(p => p.DisciplineId)
                .IsRequired()
                .HasColumnName("discipline_id");

            // Оценка не существует без студента - каскад
            builder.HasOne(p => p.Student)
                .WithMany(p => p.Grades)
                .HasForeignKey(p => p.StudentId)
                .HasConstraintName("fk_grades_student_id")
                .OnDelete(DeleteBehavior.Cascade);

            // Дисциплину с оценками удалить нельзя
            builder.HasOne(p => p.Discipline)
                .WithMany(p => p.Grades)
                .HasForeignKey(p => p.DisciplineId)
                .HasConstraintName("fk_grades_discipline_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.StudentId, "idx_grades_fk_student_id");
            builder.HasIndex(p => p.DisciplineId, "idx_grades_fk_discipline_id");
        }
    }
}
