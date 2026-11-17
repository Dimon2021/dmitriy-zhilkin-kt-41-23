using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.GradeFilters;
using DmitriyZhilkinKt_41_23.Interfaces.GradeInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DmitriyZhilkinKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        /// Средний балл по предмету в группе.
        [HttpPost("GetGroupDisciplineAverage")]
        public async Task<IActionResult> GetGroupDisciplineAverageAsync(GroupDisciplineAverageFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _gradeService.GetGroupDisciplineAverageAsync(filter, cancellationToken);
            return Ok(result);
        }

        /// Оценки конкретного студента.
        [HttpPost("GetStudentGrades")]
        public async Task<IActionResult> GetStudentGradesAsync(StudentGradesFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _gradeService.GetStudentGradesAsync(filter, cancellationToken);
            return Ok(result);
        }

        /// Средний балл по году обучения (курсу).
        [HttpPost("GetYearAverage")]
        public async Task<IActionResult> GetYearAverageAsync(YearAverageFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _gradeService.GetYearAverageAsync(filter, cancellationToken);
            return Ok(result);
        }

        /// Студенты специальности, у которых есть двойка по любой дисциплине (без повторений).
        [HttpPost("GetStudentsWithTwos")]
        public async Task<IActionResult> GetStudentsWithTwosAsync(SpecialtyStudentsFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _gradeService.GetStudentsWithTwosBySpecialtyAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddGradeAsync(GradeRequest request, CancellationToken cancellationToken = default)
        {
            var gradeId = await _gradeService.AddGradeAsync(request, cancellationToken);
            return Ok(gradeId);
        }

        [HttpPut("Update/{gradeId:int}")]
        public async Task<IActionResult> UpdateGradeAsync(int gradeId, GradeUpdateRequest request, CancellationToken cancellationToken = default)
        {
            await _gradeService.UpdateGradeAsync(gradeId, request, cancellationToken);
            return Ok();
        }
    }
}
