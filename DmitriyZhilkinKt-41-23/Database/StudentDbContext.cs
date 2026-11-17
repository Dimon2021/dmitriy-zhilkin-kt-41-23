using DmitriyZhilkinKt_41_23.Database.Configurations;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Database
{
    public class StudentDbContext : DbContext
    {
        public DbSet<Specialty> Specialties => Set<Specialty>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Discipline> Disciplines => Set<Discipline>();
        public DbSet<Grade> Grades => Set<Grade>();

        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SpecialtyConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
            modelBuilder.ApplyConfiguration(new GradeConfiguration());
        }
    }
}
