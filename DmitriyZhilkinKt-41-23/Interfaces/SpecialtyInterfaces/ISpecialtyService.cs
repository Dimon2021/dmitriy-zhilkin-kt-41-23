using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.SpecialtyFilters;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Interfaces.SpecialtyInterfaces
{
    public interface ISpecialtyService
    {
        public Task<SpecialtyResponse[]> GetSpecialtiesAsync(SpecialtyFilter filter, CancellationToken cancellationToken);
        public Task<int> AddSpecialtyAsync(SpecialtyRequest request, CancellationToken cancellationToken);
        public Task UpdateSpecialtyAsync(int specialtyId, SpecialtyRequest request, CancellationToken cancellationToken);
        public Task DeleteSpecialtyAsync(int specialtyId, CancellationToken cancellationToken);
    }

    public class SpecialtyService : ISpecialtyService
    {
        private readonly StudentDbContext _dbContext;

        public SpecialtyService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<SpecialtyResponse[]> GetSpecialtiesAsync(SpecialtyFilter filter, CancellationToken cancellationToken = default)
        {
            IQueryable<Specialty> specialties = _dbContext.Set<Specialty>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                var title = filter.Title.ToLower();
                specialties = specialties.Where(w => w.Title.ToLower().Contains(title));
            }

            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                var code = filter.Code.ToLower();
                specialties = specialties.Where(w => w.Code.ToLower().Contains(code));
            }

            return specialties
                .OrderBy(o => o.SpecialtyId)
                .Select(s => new SpecialtyResponse
                {
                    SpecialtyId = s.SpecialtyId,
                    Title = s.Title,
                    Code = s.Code
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<int> AddSpecialtyAsync(SpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(request, 0, cancellationToken);

            var specialty = new Specialty
            {
                Title = request.Title.Trim(),
                Code = request.Code.Trim()
            };

            await _dbContext.Set<Specialty>().AddAsync(specialty, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return specialty.SpecialtyId;
        }

        public async Task UpdateSpecialtyAsync(int specialtyId, SpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            var specialty = await _dbContext.Set<Specialty>()
                .FirstOrDefaultAsync(f => f.SpecialtyId == specialtyId, cancellationToken);

            if (specialty == null)
            {
                throw new KeyNotFoundException($"Специальность с id {specialtyId} не найдена");
            }

            await ValidateAsync(request, specialtyId, cancellationToken);

            specialty.Title = request.Title.Trim();
            specialty.Code = request.Code.Trim();

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteSpecialtyAsync(int specialtyId, CancellationToken cancellationToken = default)
        {
            var specialty = await _dbContext.Set<Specialty>()
                .FirstOrDefaultAsync(f => f.SpecialtyId == specialtyId, cancellationToken);

            if (specialty == null)
            {
                throw new KeyNotFoundException($"Специальность с id {specialtyId} не найдена");
            }

            // У специальности нет признака IsDeleted, а в БД стоит Restrict:
            // удалить можно только специальность, на которую не ссылается ни одна группа
            // (в том числе помеченная как удалённая).
            var hasGroups = await _dbContext.Set<Group>()
                .AnyAsync(a => a.SpecialtyId == specialtyId, cancellationToken);
            if (hasGroups)
            {
                throw new ArgumentException("Нельзя удалить специальность: к ней привязаны группы");
            }

            _dbContext.Set<Specialty>().Remove(specialty);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAsync(SpecialtyRequest request, int currentSpecialtyId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ArgumentException("Название специальности не может быть пустым");
            }

            if (request.Title.Trim().Length > 200)
            {
                throw new ArgumentException("Название специальности не должно быть длиннее 200 символов");
            }

            if (string.IsNullOrWhiteSpace(request.Code))
            {
                throw new ArgumentException("Код специальности не может быть пустым");
            }

            if (request.Code.Trim().Length > 20)
            {
                throw new ArgumentException("Код специальности не должен быть длиннее 20 символов");
            }

            var code = request.Code.Trim();
            var codeExists = await _dbContext.Set<Specialty>()
                .AnyAsync(a => a.Code == code && a.SpecialtyId != currentSpecialtyId, cancellationToken);
            if (codeExists)
            {
                throw new ArgumentException($"Специальность с кодом {code} уже существует");
            }
        }
    }
}
