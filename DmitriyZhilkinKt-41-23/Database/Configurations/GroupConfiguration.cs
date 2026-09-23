using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DmitriyZhilkinKt_41_23.Database.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(g => g.GroupId);
            builder.Property(g => g.Name).IsRequired().HasMaxLength(50);

            builder.HasOne(g => g.Speciality)
                   .WithMany(s => s.Groups)
                   .HasForeignKey(g => g.SpecialityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
