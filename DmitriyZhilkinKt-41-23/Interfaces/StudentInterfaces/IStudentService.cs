using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Filters.StudentFilters;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Interfaces.StudentInterfaces
{
    public interface IStudentService
    {
        public Task<Student[]> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken);
    }

    // В учебных целях реализация лежит рядом с интерфейсом.
    // В реальных проектах она находится в отдельном проекте с модификатором internal.
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
    }
}
