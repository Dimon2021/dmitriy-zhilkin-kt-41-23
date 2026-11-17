using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.SpecialtyFilters;
using DmitriyZhilkinKt_41_23.Interfaces.SpecialtyInterfaces;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Tests
{
    public class SpecialtyServiceIntegrationTests
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public SpecialtyServiceIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task AddSpecialtyAsync_CorrectData_SpecialtySaved()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new SpecialtyService(ctx);

            // Act
            var id = await service.AddSpecialtyAsync(
                new SpecialtyRequest { Title = "Прикладная информатика", Code = "09.03.03" },
                CancellationToken.None);

            // Assert
            Assert.True(id > 0);
            Assert.Equal(1, await ctx.Set<Specialty>().AsNoTracking().CountAsync());
        }

        [Fact]
        public async Task AddSpecialtyAsync_DuplicateCode_ThrowsArgumentException()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new SpecialtyService(ctx);
            await service.AddSpecialtyAsync(
                new SpecialtyRequest { Title = "Прикладная информатика", Code = "09.03.03" },
                CancellationToken.None);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddSpecialtyAsync(
                new SpecialtyRequest { Title = "Другая", Code = "09.03.03" },
                CancellationToken.None));
        }

        [Fact]
        public async Task GetSpecialtiesAsync_CodeFilter_OneObject()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new SpecialtyService(ctx);
            await ctx.Set<Specialty>().AddRangeAsync(
                new Specialty { Title = "Прикладная информатика", Code = "09.03.03" },
                new Specialty { Title = "Экономика", Code = "38.03.01" });
            await ctx.SaveChangesAsync();

            // Act
            var result = await service.GetSpecialtiesAsync(new SpecialtyFilter { Code = "38.03" }, CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("Экономика", result[0].Title);
        }

        [Fact]
        public async Task UpdateSpecialtyAsync_NewTitle_TitleChanged()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new SpecialtyService(ctx);
            var specialty = new Specialty { Title = "Старое", Code = "09.03.03" };
            await ctx.Set<Specialty>().AddAsync(specialty);
            await ctx.SaveChangesAsync();

            // Act
            await service.UpdateSpecialtyAsync(
                specialty.SpecialtyId,
                new SpecialtyRequest { Title = "Новое", Code = "09.03.03" },
                CancellationToken.None);

            // Assert
            var updated = await ctx.Set<Specialty>().AsNoTracking().FirstAsync();
            Assert.Equal("Новое", updated.Title);
        }

        [Fact]
        public async Task DeleteSpecialtyAsync_WithoutGroups_SpecialtyRemoved()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new SpecialtyService(ctx);
            var specialty = new Specialty { Title = "Прикладная информатика", Code = "09.03.03" };
            await ctx.Set<Specialty>().AddAsync(specialty);
            await ctx.SaveChangesAsync();

            // Act
            await service.DeleteSpecialtyAsync(specialty.SpecialtyId, CancellationToken.None);

            // Assert
            Assert.Equal(0, await ctx.Set<Specialty>().AsNoTracking().CountAsync());
        }

        [Fact]
        public async Task DeleteSpecialtyAsync_WithGroups_ThrowsArgumentException()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new SpecialtyService(ctx);
            var specialty = new Specialty { Title = "Прикладная информатика", Code = "09.03.03" };
            await ctx.Set<Specialty>().AddAsync(specialty);
            await ctx.Set<Group>().AddAsync(new Group { Name = "KT-31-20", Course = 3, Specialty = specialty });
            await ctx.SaveChangesAsync();

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.DeleteSpecialtyAsync(specialty.SpecialtyId, CancellationToken.None));
        }
    }
}
