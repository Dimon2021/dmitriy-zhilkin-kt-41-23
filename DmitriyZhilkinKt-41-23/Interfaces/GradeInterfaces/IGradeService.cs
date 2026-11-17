using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.GradeFilters;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Interfaces.GradeInterfaces
{
    public interface IGradeService
    {
        public Task<AverageResponse> GetGroupDisciplineAverageAsync(GroupDisciplineAverageFilter filter, CancellationToken cancellationToken);
        public Task<GradeResponse[]> GetStudentGradesAsync(StudentGradesFilter filter, CancellationToken cancellationToken);
        public Task<AverageResponse> GetYearAverageAsync(YearAverageFilter filter, CancellationToken cancellationToken);
        public Task<StudentResponse[]> GetStudentsWithTwosBySpecialtyAsync(SpecialtyStudentsFilter filter, CancellationToken cancellationToken);
        public Task<int> AddGradeAsync(GradeRequest request, CancellationToken cancellationToken);
        public Task UpdateGradeAsync(int gradeId, GradeUpdateRequest request, CancellationToken cancellationToken);
    }

    public class GradeService : IGradeService
    {
        public const int MinGrade = 2;
        public const int MaxGrade = 5;
        public const int TwoGrade = 2;

        private readonly StudentDbContext _dbContext;

        public GradeService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AverageResponse> GetGroupDisciplineAverageAsync(GroupDisciplineAverageFilter filter, CancellationToken cancellationToken = default)
        {
            if (!await _dbContext.Set<Group>().AnyAsync(a => a.GroupId == filter.GroupId, cancellationToken))
            {
                throw new KeyNotFoundException($"Группа с id {filter.GroupId} не найдена");
            }

            if (!await _dbContext.Set<Discipline>().AnyAsync(a => a.DisciplineId == filter.DisciplineId, cancellationToken))
            {
                throw new KeyNotFoundException($"Дисциплина с id {filter.DisciplineId} не найдена");
            }

            var values = await _dbContext.Set<Grade>()
                .AsNoTracking()
                .Where(w => w.DisciplineId == filter.DisciplineId
                            && w.Student.GroupId == filter.GroupId
                            && !w.Student.IsDeleted)
                .Select(s => s.Value)
                .ToArrayAsync(cancellationToken);

            return CreateAverage(values);
        }

        public async Task<GradeResponse[]> GetStudentGradesAsync(StudentGradesFilter filter, CancellationToken cancellationToken = default)
        {
            if (!await _dbContext.Set<Student>().AnyAsync(a => a.StudentId == filter.StudentId, cancellationToken))
            {
                throw new KeyNotFoundException($"Студент с id {filter.StudentId} не найден");
            }

            IQueryable<Grade> grades = _dbContext.Set<Grade>()
                .AsNoTracking()
                .Where(w => w.StudentId == filter.StudentId);

            if (filter.DisciplineId.HasValue)
            {
                grades = grades.Where(w => w.DisciplineId == filter.DisciplineId.Value);
            }

            return await grades
                .OrderBy(o => o.GradeId)
                .Select(s => new GradeResponse
                {
                    GradeId = s.GradeId,
                    Value = s.Value,
                    StudentId = s.StudentId,
                    DisciplineId = s.DisciplineId,
                    DisciplineName = s.Discipline.Name
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<AverageResponse> GetYearAverageAsync(YearAverageFilter filter, CancellationToken cancellationToken = default)
        {
            // "Год" = курс группы, в которой учится студент
            var values = await _dbContext.Set<Grade>()
                .AsNoTracking()
                .Where(w => w.Student.Group.Course == filter.Course && !w.Student.IsDeleted)
                .Select(s => s.Value)
                .ToArrayAsync(cancellationToken);

            return CreateAverage(values);
        }

        public async Task<StudentResponse[]> GetStudentsWithTwosBySpecialtyAsync(SpecialtyStudentsFilter filter, CancellationToken cancellationToken = default)
        {
            if (!await _dbContext.Set<Specialty>().AnyAsync(a => a.SpecialtyId == filter.SpecialtyId, cancellationToken))
            {
                throw new KeyNotFoundException($"Специальность с id {filter.SpecialtyId} не найдена");
            }

            // Any() проверяет наличие двойки, поэтому каждый студент попадает в список один раз,
            // сколько бы двоек у него ни было
            return await _dbContext.Set<Student>()
                .AsNoTracking()
                .Where(w => !w.IsDeleted
                            && !w.Group.IsDeleted
                            && w.Group.SpecialtyId == filter.SpecialtyId
                            && w.Grades.Any(a => a.Value == TwoGrade))
                .OrderBy(o => o.Group.Name)
                .ThenBy(o => o.LastName)
                .ThenBy(o => o.FirstName)
                .Select(s => new StudentResponse
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    GroupId = s.GroupId,
                    GroupName = s.Group.Name
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<int> AddGradeAsync(GradeRequest request, CancellationToken cancellationToken = default)
        {
            ValidateValue(request.Value);

            var studentExists = await _dbContext.Set<Student>()
                .AnyAsync(a => a.StudentId == request.StudentId && !a.IsDeleted, cancellationToken);
            if (!studentExists)
            {
                throw new ArgumentException($"Студент с id {request.StudentId} не найден");
            }

            var disciplineExists = await _dbContext.Set<Discipline>()
                .AnyAsync(a => a.DisciplineId == request.DisciplineId && !a.IsDeleted, cancellationToken);
            if (!disciplineExists)
            {
                throw new ArgumentException($"Дисциплина с id {request.DisciplineId} не найдена");
            }

            var grade = new Grade
            {
                StudentId = request.StudentId,
                DisciplineId = request.DisciplineId,
                Value = request.Value
            };

            await _dbContext.Set<Grade>().AddAsync(grade, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return grade.GradeId;
        }

        public async Task UpdateGradeAsync(int gradeId, GradeUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var grade = await _dbContext.Set<Grade>()
                .FirstOrDefaultAsync(f => f.GradeId == gradeId, cancellationToken);

            if (grade == null)
            {
                throw new KeyNotFoundException($"Оценка с id {gradeId} не найдена");
            }

            ValidateValue(request.Value);

            grade.Value = request.Value;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private static void ValidateValue(int value)
        {
            if (value < MinGrade || value > MaxGrade)
            {
                throw new ArgumentException($"Оценка должна быть от {MinGrade} до {MaxGrade}");
            }
        }

        private static AverageResponse CreateAverage(int[] values)
        {
            return new AverageResponse
            {
                GradesCount = values.Length,
                Average = values.Length == 0 ? null : Math.Round(values.Average(), 2)
            };
        }
    }
}
