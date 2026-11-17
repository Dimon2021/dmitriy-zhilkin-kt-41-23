using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.GradeFilters;
using DmitriyZhilkinKt_41_23.Interfaces.GradeInterfaces;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Tests
{
    public class GradeServiceIntegrationTests
    {
        private readonly DbContextOptions<StudentDbContext> _dbContextOptions;

        public GradeServiceIntegrationTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<StudentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private class SeedData
        {
            public Group Group3 { get; set; } = null!;   // KT-31-20, 3 курс
            public Group Group4 { get; set; } = null!;   // KT-41-20, 4 курс
            public Student Student1 { get; set; } = null!;
            public Student Student2 { get; set; } = null!;
            public Student Student3 { get; set; } = null!;
            public Discipline Math { get; set; } = null!;
            public Discipline History { get; set; } = null!;
        }

        /// <summary>
        /// KT-31-20 (3 курс): s1, s2. KT-41-20 (4 курс): s3.
        /// Оценки: s1-Math 5, s2-Math 3, s3-Math 4, s1-History 5.
        /// </summary>
        private static async Task<SeedData> SeedAsync(StudentDbContext ctx)
        {
            var specialty = new Specialty { Title = "Прикладная информатика", Code = "09.03.03" };
            var group3 = new Group { Name = "KT-31-20", Course = 3, Specialty = specialty };
            var group4 = new Group { Name = "KT-41-20", Course = 4, Specialty = specialty };

            var s1 = new Student { FirstName = "Иван", LastName = "Иванов", Group = group3 };
            var s2 = new Student { FirstName = "Пётр", LastName = "Петров", Group = group3 };
            var s3 = new Student { FirstName = "Анна", LastName = "Сидорова", Group = group4 };

            var math = new Discipline { Name = "Математика", Direction = DisciplineDirection.Technical };
            var history = new Discipline { Name = "История", Direction = DisciplineDirection.Humanities };

            await ctx.Set<Specialty>().AddAsync(specialty);
            await ctx.Set<Group>().AddRangeAsync(group3, group4);
            await ctx.Set<Student>().AddRangeAsync(s1, s2, s3);
            await ctx.Set<Discipline>().AddRangeAsync(math, history);
            await ctx.Set<Grade>().AddRangeAsync(
                new Grade { Student = s1, Discipline = math, Value = 5 },
                new Grade { Student = s2, Discipline = math, Value = 3 },
                new Grade { Student = s3, Discipline = math, Value = 4 },
                new Grade { Student = s1, Discipline = history, Value = 5 });
            await ctx.SaveChangesAsync();

            return new SeedData { Group3 = group3, Group4 = group4, Student1 = s1, Student2 = s2, Student3 = s3, Math = math, History = history };
        }

        [Fact]
        public async Task GetGroupDisciplineAverageAsync_Group3Math_Average4()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act
            var result = await service.GetGroupDisciplineAverageAsync(
                new GroupDisciplineAverageFilter { GroupId = data.Group3.GroupId, DisciplineId = data.Math.DisciplineId },
                CancellationToken.None);

            // Assert
            Assert.Equal(2, result.GradesCount);
            Assert.Equal(4.0, result.Average!.Value);
        }

        [Fact]
        public async Task GetGroupDisciplineAverageAsync_Group4History_NoGradesAverageNull()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act
            var result = await service.GetGroupDisciplineAverageAsync(
                new GroupDisciplineAverageFilter { GroupId = data.Group4.GroupId, DisciplineId = data.History.DisciplineId },
                CancellationToken.None);

            // Assert
            Assert.Equal(0, result.GradesCount);
            Assert.Null(result.Average);
        }

        [Fact]
        public async Task GetYearAverageAsync_Course3_Average4Point33()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            await SeedAsync(ctx);

            // Act - оценки 3 курса: 5, 3, 5
            var result = await service.GetYearAverageAsync(new YearAverageFilter { Course = 3 }, CancellationToken.None);

            // Assert
            Assert.Equal(3, result.GradesCount);
            Assert.Equal(4.33, result.Average!.Value, 2);
        }

        [Fact]
        public async Task GetStudentGradesAsync_Student1_TwoObjects()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act
            var result = await service.GetStudentGradesAsync(
                new StudentGradesFilter { StudentId = data.Student1.StudentId },
                CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public async Task GetStudentGradesAsync_Student1History_OneObject()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act
            var result = await service.GetStudentGradesAsync(
                new StudentGradesFilter { StudentId = data.Student1.StudentId, DisciplineId = data.History.DisciplineId },
                CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.Equal("История", result[0].DisciplineName);
            Assert.Equal(5, result[0].Value);
        }

        [Fact]
        public async Task GetStudentsWithTwosBySpecialtyAsync_StudentWithSeveralTwos_ReturnedOnce()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);
            // у Петрова две двойки (по двум дисциплинам), у Сидоровой одна
            await ctx.Set<Grade>().AddRangeAsync(
                new Grade { Student = data.Student2, Discipline = data.Math, Value = 2 },
                new Grade { Student = data.Student2, Discipline = data.History, Value = 2 },
                new Grade { Student = data.Student3, Discipline = data.Math, Value = 2 });
            await ctx.SaveChangesAsync();

            // Act
            var result = await service.GetStudentsWithTwosBySpecialtyAsync(
                new SpecialtyStudentsFilter { SpecialtyId = data.Group3.SpecialtyId },
                CancellationToken.None);

            // Assert - Иванов (только пятёрки) не попал, Петров один раз, Сидорова один раз
            Assert.Equal(2, result.Length);
            Assert.Single(result, r => r.LastName == "Петров");
            Assert.Single(result, r => r.LastName == "Сидорова");
        }

        [Fact]
        public async Task GetStudentsWithTwosBySpecialtyAsync_NoTwos_ZeroObjects()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act
            var result = await service.GetStudentsWithTwosBySpecialtyAsync(
                new SpecialtyStudentsFilter { SpecialtyId = data.Group3.SpecialtyId },
                CancellationToken.None);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetStudentsWithTwosBySpecialtyAsync_UnknownSpecialty_ThrowsKeyNotFoundException()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);

            // Act + Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetStudentsWithTwosBySpecialtyAsync(
                new SpecialtyStudentsFilter { SpecialtyId = 999 },
                CancellationToken.None));
        }

        [Fact]
        public async Task AddGradeAsync_CorrectData_GradeSaved()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act
            var gradeId = await service.AddGradeAsync(
                new GradeRequest { StudentId = data.Student1.StudentId, DisciplineId = data.Math.DisciplineId, Value = 4 },
                CancellationToken.None);

            // Assert
            Assert.True(gradeId > 0);
            Assert.Equal(5, await ctx.Set<Grade>().AsNoTracking().CountAsync());
        }

        [Fact]
        public async Task AddGradeAsync_ValueSix_ThrowsArgumentException()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);

            // Act + Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddGradeAsync(
                new GradeRequest { StudentId = data.Student1.StudentId, DisciplineId = data.Math.DisciplineId, Value = 6 },
                CancellationToken.None));
        }

        [Fact]
        public async Task UpdateGradeAsync_NewValue_ValueChanged()
        {
            // Arrange
            await using var ctx = new StudentDbContext(_dbContextOptions);
            var service = new GradeService(ctx);
            var data = await SeedAsync(ctx);
            var grade = await ctx.Set<Grade>().AsNoTracking()
                .FirstAsync(f => f.StudentId == data.Student1.StudentId && f.DisciplineId == data.Math.DisciplineId);

            // Act
            await service.UpdateGradeAsync(grade.GradeId, new GradeUpdateRequest { Value = 2 }, CancellationToken.None);

            // Assert
            var updated = await ctx.Set<Grade>().AsNoTracking().FirstAsync(f => f.GradeId == grade.GradeId);
            Assert.Equal(2, updated.Value);
        }
    }
}
