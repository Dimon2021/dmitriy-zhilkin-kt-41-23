using DmitriyZhilkinKt_41_23.Dto;
using DmitriyZhilkinKt_41_23.Filters.StudentFilters;
using DmitriyZhilkinKt_41_23.Interfaces.StudentInterfaces;
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

        [HttpPost("Add")]
        public async Task<IActionResult> AddStudentAsync(StudentRequest request, CancellationToken cancellationToken = default)
        {
            var studentId = await _studentService.AddStudentAsync(request, cancellationToken);
            return Ok(studentId);
        }

        [HttpPut("Update/{studentId:int}")]
        public async Task<IActionResult> UpdateStudentAsync(int studentId, StudentRequest request, CancellationToken cancellationToken = default)
        {
            await _studentService.UpdateStudentAsync(studentId, request, cancellationToken);
            return Ok();
        }

        [HttpDelete("Delete/{studentId:int}")]
        public async Task<IActionResult> DeleteStudentAsync(int studentId, CancellationToken cancellationToken = default)
        {
            await _studentService.DeleteStudentAsync(studentId, cancellationToken);
            return Ok();
        }
    }
}
