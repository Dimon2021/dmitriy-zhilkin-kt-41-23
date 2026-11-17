using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.StudentFilters;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Interfaces.StudentInterfaces
{
    public interface IStudentService
    {
        public Task<Student[]> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken);
        public Task<int> AddStudentAsync(StudentRequest request, CancellationToken cancellationToken);
        public Task UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken cancellationToken);
        public Task DeleteStudentAsync(int studentId, CancellationToken cancellationToken);
    }

    
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _dbContext;

        public StudentService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Student[]> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken = default)
        {
            var students = _dbContext.Set<Student>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.GroupName))
            {
                students = students.Where(w => w.Group.Name == filter.GroupName);
            }

            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                var firstName = filter.FirstName.ToLower();
                students = students.Where(w => w.FirstName.ToLower().Contains(firstName));
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                var lastName = filter.LastName.ToLower();
                students = students.Where(w => w.LastName.ToLower().Contains(lastName));
            }

            if (filter.IsDeleted.HasValue)
            {
                students = students.Where(w => w.IsDeleted == filter.IsDeleted.Value);
            }

            return students.AsNoTracking().ToArrayAsync(cancellationToken);
        }

        public async Task<int> AddStudentAsync(StudentRequest request, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(request, cancellationToken);

            var student = new Student
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                GroupId = request.GroupId
            };

            await _dbContext.Set<Student>().AddAsync(student, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return student.StudentId;
        }

        public async Task UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken cancellationToken = default)
        {
            var student = await _dbContext.Set<Student>()
                .FirstOrDefaultAsync(f => f.StudentId == studentId && !f.IsDeleted, cancellationToken);

            if (student == null)
            {
                throw new KeyNotFoundException($"Студент с id {studentId} не найден");
            }

            await ValidateAsync(request, cancellationToken);

            student.FirstName = request.FirstName.Trim();
            student.LastName = request.LastName.Trim();
            student.GroupId = request.GroupId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteStudentAsync(int studentId, CancellationToken cancellationToken = default)
        {
            var student = await _dbContext.Set<Student>()
                .FirstOrDefaultAsync(f => f.StudentId == studentId && !f.IsDeleted, cancellationToken);

            if (student == null)
            {
                throw new KeyNotFoundException($"Студент с id {studentId} не найден");
            }

            
            student.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAsync(StudentRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                throw new ArgumentException("Имя студента не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                throw new ArgumentException("Фамилия студента не может быть пустой");
            }

            var groupExists = await _dbContext.Set<Group>()
                .AnyAsync(a => a.GroupId == request.GroupId && !a.IsDeleted, cancellationToken);
            if (!groupExists)
            {
                throw new ArgumentException($"Группа с id {request.GroupId} не найдена");
            }
        }
    }
}
