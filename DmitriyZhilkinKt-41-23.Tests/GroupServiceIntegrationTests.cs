using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.GroupFilters;
using DmitriyZhilkinKt_41_23.Interfaces.GroupInterfaces;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Tests
{
    public class GroupServiceIntegrationTests
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public GroupServiceIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private static async Task<Specialty> SeedSpecialtyAsync(StudentDbContext ctx)
        {
            var specialty = new Specialty { Title = "Прикладная информатика", Code = "09.03.03" };
            await ctx.Set<Specialty>().AddAsync(specialty);
            await ctx.SaveChangesAsync();
            return specialty;
        }

        [Fact]
        public async Task GetGroupsAsync_Course3_OneObject()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GroupService(ctx);
            var specialty = await SeedSpecialtyAsync(ctx);
            await ctx.Set<Group>().AddRangeAsync(
                new Group { Name = "KT-31-20", Course = 3, Specialty = specialty },
                new Group { Name = "KT-41-20", Course = 4, Specialty = specialty });
            await ctx.SaveChangesAsync();

            // Act
            var result = await service.GetGroupsAsync(new GroupFilter { Course = 3 }, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("KT-31-20", result[0].Name);
            Assert.Equal("Прикладная информатика", result[0].SpecialtyTitle);
        }

        [Fact]
        public async Task GetGroupsAsync_IsDeletedTrue_OneObject()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GroupService(ctx);
            var specialty = await SeedSpecialtyAsync(ctx);
            await ctx.Set<Group>().AddRangeAsync(
                new Group { Name = "KT-31-20", Course = 3, Specialty = specialty },
                new Group { Name = "KT-41-20", Course = 4, Specialty = specialty, IsDeleted = true });
            await ctx.SaveChangesAsync();

            // Act
            var result = await service.GetGroupsAsync(new GroupFilter { IsDeleted = true }, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("KT-41-20", result[0].Name);
        }

        [Fact]
        public async Task AddGroupAsync_CorrectData_GroupSaved()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GroupService(ctx);
            var specialty = await SeedSpecialtyAsync(ctx);

            // Act
            var groupId = await service.AddGroupAsync(
                new GroupRequest { Name = "KT-31-20", Course = 3, SpecialtyId = specialty.SpecialtyId },
                CancellationToken.None);

            // Assert
            Assert.True(groupId > 0);
            Assert.Equal(1, await ctx.Set<Group>().AsNoTracking().CountAsync());
        }

        [Fact]
        public async Task AddGroupAsync_InvalidName_ThrowsArgumentException()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GroupService(ctx);
            var specialty = await SeedSpecialtyAsync(ctx);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddGroupAsync(
                new GroupRequest { Name = "KT3120", Course = 3, SpecialtyId = specialty.SpecialtyId },
                CancellationToken.None));
        }

        [Fact]
        public async Task DeleteGroupAsync_GroupWithStudents_GroupAndStudentsMarkedDeleted()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GroupService(ctx);
            var specialty = await SeedSpecialtyAsync(ctx);
            var group = new Group { Name = "KT-31-20", Course = 3, Specialty = specialty };
            await ctx.Set<Group>().AddAsync(group);
            await ctx.Set<Student>().AddRangeAsync(
                new Student { FirstName = "Иван", LastName = "Иванов", Group = group },
                new Student { FirstName = "Пётр", LastName = "Петров", Group = group });
            await ctx.SaveChangesAsync();

            // Act
            await service.DeleteGroupAsync(group.GroupId, CancellationToken.None);

            // Assert
            var groups = await ctx.Set<Group>().AsNoTracking().ToArrayAsync();
            var students = await ctx.Set<Student>().AsNoTracking().ToArrayAsync();
            Assert.All(groups, g => Assert.True(g.IsDeleted));
            Assert.Equal(2, students.Length);
            Assert.All(students, s => Assert.True(s.IsDeleted));
        }

        [Fact]
        public async Task DeleteGroupAsync_UnknownId_ThrowsKeyNotFoundException()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GroupService(ctx);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteGroupAsync(999, CancellationToken.None));
        }
    }
}
