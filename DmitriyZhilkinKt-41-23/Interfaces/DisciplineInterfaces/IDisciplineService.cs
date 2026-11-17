using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.DisciplineFilters;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Interfaces.DisciplineInterfaces
{
    public interface IDisciplineService
    {
        public Task<DisciplineResponse[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken);
        public Task<int> AddDisciplineAsync(DisciplineRequest request, CancellationToken cancellationToken);
        public Task UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken cancellationToken);
        public Task DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken);
    }

    public class DisciplineService : IDisciplineService
    {
        private readonly StudentDbContext _dbContext;

        public DisciplineService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<DisciplineResponse[]> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken = default)
        {
            IQueryable<Discipline> disciplines = _dbContext.Set<Discipline>().AsNoTracking();

            if (filter.Direction.HasValue)
            {
                disciplines = disciplines.Where(w => w.Direction == filter.Direction.Value);
            }

            if (filter.IsDeleted.HasValue)
            {
                disciplines = disciplines.Where(w => w.IsDeleted == filter.IsDeleted.Value);
            }

            return disciplines
                .OrderBy(o => o.DisciplineId)
                .Select(s => new DisciplineResponse
                {
                    DisciplineId = s.DisciplineId,
                    Name = s.Name,
                    Direction = s.Direction,
                    IsDeleted = s.IsDeleted
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<int> AddDisciplineAsync(DisciplineRequest request, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(request, 0, cancellationToken);

            var discipline = new Discipline
            {
                Name = request.Name.Trim(),
                Direction = request.Direction
            };

            await _dbContext.Set<Discipline>().AddAsync(discipline, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return discipline.DisciplineId;
        }

        public async Task UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken cancellationToken = default)
        {
            var discipline = await _dbContext.Set<Discipline>()
                .FirstOrDefaultAsync(f => f.DisciplineId == disciplineId && !f.IsDeleted, cancellationToken);

            if (discipline == null)
            {
                throw new KeyNotFoundException($"Дисциплина с id {disciplineId} не найдена");
            }

            await ValidateAsync(request, disciplineId, cancellationToken);

            discipline.Name = request.Name.Trim();
            discipline.Direction = request.Direction;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken = default)
        {
            var discipline = await _dbContext.Set<Discipline>()
                .FirstOrDefaultAsync(f => f.DisciplineId == disciplineId && !f.IsDeleted, cancellationToken);

            if (discipline == null)
            {
                throw new KeyNotFoundException($"Дисциплина с id {disciplineId} не найдена");
            }

            
            discipline.IsDeleted = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAsync(DisciplineRequest request, int currentDisciplineId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Название дисциплины не может быть пустым");
            }

            if (!Enum.IsDefined(request.Direction))
            {
                throw new ArgumentException("Направление должно быть Humanities или Technical");
            }

            var name = request.Name.Trim();
            var nameExists = await _dbContext.Set<Discipline>()
                .AnyAsync(a => a.Name == name && !a.IsDeleted && a.DisciplineId != currentDisciplineId, cancellationToken);
            if (nameExists)
            {
                throw new ArgumentException($"Дисциплина {name} уже существует");
            }
        }
    }
}
