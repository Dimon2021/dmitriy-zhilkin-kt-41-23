using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        private const string TableName = "Groups";

        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(p => p.GroupId)
                .HasName($"pk_{TableName}_group_id");

            builder.Property(p => p.GroupId)
                .HasColumnName("group_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");

            builder.Property(p => p.Course)
                .IsRequired()
                .HasColumnName("course");

            builder.Property(p => p.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");

            builder.Property(p => p.SpecialtyId)
                .IsRequired()
                .HasColumnName("specialty_id");

            // Специальность одна, групп в ней много.
            // Restrict: нельзя удалить специальность, пока в ней есть группы.
            builder.HasOne(p => p.Specialty)
                .WithMany(p => p.Groups)
                .HasForeignKey(p => p.SpecialtyId)
                .HasConstraintName("fk_groups_specialty_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.SpecialtyId, "idx_groups_fk_specialty_id");
        }
    }
}
