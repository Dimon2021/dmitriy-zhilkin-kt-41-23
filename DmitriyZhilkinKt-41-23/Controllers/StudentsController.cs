using DmitriyZhilkinKt_41_23.Filters.StudentFilters;
using DmitriyZhilkinKt_41_23.Interfaces.StudentInterfaces;
using DmitriyZhilkinKt_41_23.Models;
using Microsoft.AspNetCore.Mvc;

namespace DmitriyZhilkinKt_41_23.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly IStudentService _studentService;

        public StudentsController(ILogger<StudentsController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpPost("GetStudents")]
        public async Task<IActionResult> GetStudentsAsync(StudentFilter filter, CancellationToken cancellationToken = default)
        {
            var students = await _studentService.GetStudentsAsync(filter, cancellationToken);

            return Ok(students);
        }
    }
}
