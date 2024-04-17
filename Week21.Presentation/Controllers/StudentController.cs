using Microsoft.AspNetCore.Mvc;
using Week21.Application;
using Week21.Domain;

namespace Week21.Presentation.Controllers
{
    public class StudentController : Controller
    {

        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost, Route("AddStudent")]
        public async Task<IActionResult> AddStudent(Student
            std)
        {
            var result = await _studentService.AddStudent(std);
            return Ok(result);
        }

        [HttpGet, Route("GetStudent")]
        public async Task<IActionResult> GetAStudent(Guid id)
        {
            var result = await _studentService.GetStudentById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet, Route("GetStudents")]
        public async Task<IActionResult> GetAllStudent()
        {
            var result = await _studentService.GetAllStudent();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete, Route("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(Guid Id)
        {
            await _studentService.DeleteStudent(Id);
            
            return Ok();
        }

        [HttpPut, Route("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            var result = await _studentService.UpdateStudent(student);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
