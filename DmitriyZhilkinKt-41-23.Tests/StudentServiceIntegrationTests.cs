using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Filters.StudentFilters;
using DmitriyZhilkinKt_41_23.Interfaces.StudentInterfaces;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Tests
{
    public class StudentServiceIntegrationTests
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public StudentServiceIntegrationTests()
        {
            
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        
        /// 2 группы, 4 студента:
        /// KT-31-20: Иванов, Сидоров (удалён); KT-41-20: Петров, Иванова.
        
        private async Task SeedAsync(StudentDbContext ctx)
        {
            var specialty = new Specialty { Title = "Прикладная информатика", Code = "09.03.03" };

            var group1 = new Group { Name = "KT-31-20", Course = 3, Specialty = specialty };
            var group2 = new Group { Name = "KT-41-20", Course = 4, Specialty = specialty };

            await ctx.Set<Specialty>().AddAsync(specialty);
            await ctx.Set<Group>().AddRangeAsync(group1, group2);

            await ctx.Set<Student>().AddRangeAsync(
                new Student { FirstName = "Иван", LastName = "Иванов", Group = group1 },
                new Student { FirstName = "Сергей", LastName = "Сидоров", Group = group1, IsDeleted = true },
                new Student { FirstName = "Пётр", LastName = "Петров", Group = group2 },
                new Student { FirstName = "Анна", LastName = "Иванова", Group = group2 });

            await ctx.SaveChangesAsync();
        }

        [Fact]
        public async Task GetStudentsAsync_KT3120_TwoObjects()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);
            await SeedAsync(ctx);

            // Act
            var filter = new StudentFilter { GroupName = "KT-31-20" };
            var result = await studentService.GetStudentsAsync(filter, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.All(result, s => Assert.Equal(1, s.GroupId));
        }

        [Fact]
        public async Task GetStudentsAsync_EmptyFilter_FourObjects()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);
            await SeedAsync(ctx);

            // Act
            var result = await studentService.GetStudentsAsync(new StudentFilter(), CancellationToken.None);

            // Assert
            Assert.Equal(4, result.Length);
        }

        [Fact]
        public async Task GetStudentsAsync_LowercaseGroupName_ZeroObjects()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);
            await SeedAsync(ctx);

            // Act - регистр названия группы учитывается
            var filter = new StudentFilter { GroupName = "kt-31-20" };
            var result = await studentService.GetStudentsAsync(filter, CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetStudentsAsync_LastNameIvan_TwoObjects()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);
            await SeedAsync(ctx);

            // Act - поиск по части фамилии без учёта регистра: Иванов и Иванова
            var filter = new StudentFilter { LastName = "ИВАНОВ" };
            var result = await studentService.GetStudentsAsync(filter, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public async Task GetStudentsAsync_IsDeletedTrue_OneObject()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);
            await SeedAsync(ctx);

            // Act
            var filter = new StudentFilter { IsDeleted = true };
            var result = await studentService.GetStudentsAsync(filter, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("Сидоров", result[0].LastName);
        }

        [Fact]
        public async Task GetStudentsAsync_GroupKT3120AndNotDeleted_OneObject()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var studentService = new StudentService(ctx);
            await SeedAsync(ctx);

            // Act - комбинация фильтров
            var filter = new StudentFilter { GroupName = "KT-31-20", IsDeleted = false };
            var result = await studentService.GetStudentsAsync(filter, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("Иванов", result[0].LastName);
        }
    }
}
