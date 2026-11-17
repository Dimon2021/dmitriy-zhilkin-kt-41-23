using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.DisciplineFilters;
using DmitriyZhilkinKt_41_23.Interfaces.DisciplineInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DmitriyZhilkinKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisciplinesController : ControllerBase
    {
        private readonly IDisciplineService _disciplineService;

        public DisciplinesController(IDisciplineService disciplineService)
        {
            _disciplineService = disciplineService;
        }

        [HttpPost("GetDisciplines")]
        public async Task<IActionResult> GetDisciplinesAsync(DisciplineFilter filter, CancellationToken cancellationToken = default)
        {
            var disciplines = await _disciplineService.GetDisciplinesAsync(filter, cancellationToken);
            return Ok(disciplines);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddDisciplineAsync(DisciplineRequest request, CancellationToken cancellationToken = default)
        {
            var disciplineId = await _disciplineService.AddDisciplineAsync(request, cancellationToken);
            return Ok(disciplineId);
        }

        [HttpPut("Update/{disciplineId:int}")]
        public async Task<IActionResult> UpdateDisciplineAsync(int disciplineId, DisciplineRequest request, CancellationToken cancellationToken = default)
        {
            await _disciplineService.UpdateDisciplineAsync(disciplineId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("Delete/{disciplineId:int}")]
        public async Task<IActionResult> DeleteDisciplineAsync(int disciplineId, CancellationToken cancellationToken = default)
        {
            await _disciplineService.DeleteDisciplineAsync(disciplineId, cancellationToken);
            return Ok();
        }
    }
}
