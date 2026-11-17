using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.SpecialtyFilters;
using DmitriyZhilkinKt_41_23.Interfaces.SpecialtyInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace DmitriyZhilkinKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpPost("GetSpecialties")]
        public async Task<IActionResult> GetSpecialtiesAsync(SpecialtyFilter filter, CancellationToken cancellationToken = default)
        {
            var specialties = await _specialtyService.GetSpecialtiesAsync(filter, cancellationToken);
            return Ok(specialties);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddSpecialtyAsync(SpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            var specialtyId = await _specialtyService.AddSpecialtyAsync(request, cancellationToken);
            return Ok(specialtyId);
        }

        [HttpPut("Update/{specialtyId:int}")]
        public async Task<IActionResult> UpdateSpecialtyAsync(int specialtyId, SpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            await _specialtyService.UpdateSpecialtyAsync(specialtyId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("Delete/{specialtyId:int}")]
        public async Task<IActionResult> DeleteSpecialtyAsync(int specialtyId, CancellationToken cancellationToken = default)
        {
            await _specialtyService.DeleteSpecialtyAsync(specialtyId, cancellationToken);
            return Ok();
        }
    }
}
