using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        private const string TableName = "Students";

        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.StudentId)
                .HasName($"pk_{TableName}_student_id");

            builder.Property(p => p.StudentId)
                .HasColumnName("student_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("first_name");

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("last_name");

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");

            builder.Property(p => p.GroupId)
                .IsRequired()
                .HasColumnName("group_id");

            // Студент не может существовать без группы - каскадное удаление
            builder.HasOne(p => p.Group)
                .WithMany(p => p.Students)
                .HasForeignKey(p => p.GroupId)
                .HasConstraintName("fk_students_group_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.GroupId, "idx_students_fk_group_id");
        }
    }
}
