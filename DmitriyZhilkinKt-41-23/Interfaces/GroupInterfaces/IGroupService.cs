using DmitriyZhilkinKt_41_23.Database;
using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.GroupFilters;
using DmitriyZhilkinKt_41_23.Models;
using DmitriyZhilkinKt_41_23.Validators;
using Microsoft.EntityFrameworkCore;

namespace DmitriyZhilkinKt_41_23.Interfaces.GroupInterfaces
{
    public interface IGroupService
    {
        public Task<GroupResponse[]> GetGroupsAsync(GroupFilter filter, CancellationToken cancellationToken);
        public Task<int> AddGroupAsync(GroupRequest request, CancellationToken cancellationToken);
        public Task UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken cancellationToken);
        public Task DeleteGroupAsync(int groupId, CancellationToken cancellationToken);
    }

    public class GroupService : IGroupService
    {
        private readonly StudentDbContext _dbContext;

        public GroupService(StudentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<GroupResponse[]> GetGroupsAsync(GroupFilter filter, CancellationToken cancellationToken = default)
        {
            IQueryable<Group> groups = _dbContext.Set<Group>().AsNoTracking();

            if (filter.SpecialtyId.HasValue)
            {
                groups = groups.Where(w => w.SpecialtyId == filter.SpecialtyId.Value);
            }

            if (filter.Course.HasValue)
            {
                groups = groups.Where(w => w.Course == filter.Course.Value);
            }

            if (filter.IsDeleted.HasValue)
            {
                groups = groups.Where(w => w.IsDeleted == filter.IsDeleted.Value);
            }

            return groups
                .OrderBy(o => o.GroupId)
                .Select(s => new GroupResponse
                {
                    GroupId = s.GroupId,
                    Name = s.Name,
                    Course = s.Course,
                    SpecialtyId = s.SpecialtyId,
                    SpecialtyTitle = s.Specialty.Title,
                    IsDeleted = s.IsDeleted
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<int> AddGroupAsync(GroupRequest request, CancellationToken cancellationToken = default)
        {
            await ValidateAsync(request, 0, cancellationToken);

            var group = new Group
            {
                Name = request.Name,
                Course = request.Course,
                SpecialtyId = request.SpecialtyId
            };

            await _dbContext.Set<Group>().AddAsync(group, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return group.GroupId;
        }

        public async Task UpdateGroupAsync(int groupId, GroupRequest request, CancellationToken cancellationToken = default)
        {
            var group = await _dbContext.Set<Group>()
                .FirstOrDefaultAsync(f => f.GroupId == groupId && !f.IsDeleted, cancellationToken);

            if (group == null)
            {
                throw new KeyNotFoundException($"Группа с id {groupId} не найдена");
            }

            await ValidateAsync(request, groupId, cancellationToken);

            group.Name = request.Name;
            group.Course = request.Course;
            group.SpecialtyId = request.SpecialtyId;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteGroupAsync(int groupId, CancellationToken cancellationToken = default)
        {
            var group = await _dbContext.Set<Group>()
                .Include(i => i.Students)
                .FirstOrDefaultAsync(f => f.GroupId == groupId && !f.IsDeleted, cancellationToken);

            if (group == null)
            {
                throw new KeyNotFoundException($"Группа с id {groupId} не найдена");
            }

            
            group.IsDeleted = true;
            foreach (var student in group.Students)
            {
                student.IsDeleted = true;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAsync(GroupRequest request, int currentGroupId, CancellationToken cancellationToken)
        {
            if (!GroupNameValidator.IsValid(request.Name))
            {
                throw new ArgumentException("Название группы не соответствует шаблону, например: KT-31-20");
            }

            if (request.Course < 1 || request.Course > 6)
            {
                throw new ArgumentException("Курс должен быть от 1 до 6");
            }

            var specialtyExists = await _dbContext.Set<Specialty>()
                .AnyAsync(a => a.SpecialtyId == request.SpecialtyId, cancellationToken);
            if (!specialtyExists)
            {
                throw new ArgumentException($"Специальность с id {request.SpecialtyId} не найдена");
            }

            var nameExists = await _dbContext.Set<Group>()
                .AnyAsync(a => a.Name == request.Name && !a.IsDeleted && a.GroupId != currentGroupId, cancellationToken);
            if (nameExists)
            {
                throw new ArgumentException($"Группа {request.Name} уже существует");
            }
        }
    }
}
