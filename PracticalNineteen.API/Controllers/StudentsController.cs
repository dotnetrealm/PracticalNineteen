using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticalNineteen.Domain.DTO;
using PracticalNineteen.Domain.Interfaces;

namespace PracticalNineteen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {

            IEnumerable<StudentModel> student = await _studentRepository.GetAllStudentsAsync();
            return Ok(student);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            StudentModel student = await _studentRepository.GetStudentByIdAsync(id);
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] StudentModel student)
        {
            if (ModelState.IsValid)
            {
                if (student == null) return BadRequest(ModelState);
                int studentId = await _studentRepository.InsertStudentAsync(student);
                student.Id = studentId;
                return CreatedAtAction("GetStudentById", new { id = studentId }, student);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] StudentModel student)
        {
            if (student == null) return BadRequest(ModelState);
            bool isUpdated = await _studentRepository.UpdateStudentAsync(id, student);

            if (!isUpdated) return NotFound();
            return Ok(student);
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();
            bool isDeleted = await _studentRepository.DeleteStudentAsync(id);
            if (!isDeleted) return NotFound();
            return Ok();
        }
    }
}
