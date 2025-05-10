using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.API.Controllers
{
    [ApiController]
    [Route("api/migration")]
    public class MigrationController : ControllerBase
    {
        private static MigrationSystem _system;

        // Initialization of the migration system
        [HttpPost("init")]
        public IActionResult Init([FromBody] MigrationInitRequest request)
        {
            if (request.Calls <= 0 || request.Departments == null || request.Students == null)
                return BadRequest("Invalid initialization data.");

            var departments = request.Departments
                .Select(d => new Department(d.Name, d.Capacity))
                .ToList();

            var students = request.Students
                .Select(s => new Student(
                    s.Id,
                    s.Name,
                    s.Rank,
                    s.Choices
                        .Select(choiceName => departments.FirstOrDefault(d => d.Name == choiceName))
                        .Where(d => d != null)
                        .ToList()
                ))
                .ToList();

            var selectionStrategy = new RankFirstStrategy();  
            var assignmentStrategy = new InitialAssignmentStrategy(); 

            _system = new MigrationSystem(request.Calls, selectionStrategy, assignmentStrategy);

            _system.Departments.AddRange(departments);
            _system.Students.AddRange(students);

            _system.Initialize();

            return Ok("Migration system initialized successfully.");
        }

        // Run the next migration call
        [HttpPost("call")]
        public IActionResult Call()
        {
            if (_system == null)
                return BadRequest("Migration system is not initialized.");

            _system.RunNextCall();
            return Ok("Migration round completed.");
        }

        // Finalize a student's migration
        [HttpPost("finalize/{studentId}")]
        public IActionResult Finalize(int studentId)
        {
            if (_system == null)
                return BadRequest("Migration system is not initialized.");

            var student = _system.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
                return NotFound($"Student with ID {studentId} not found.");

            _system.FinalizeStudent(studentId);
            return Ok($"Migration turned off for student {student.Name}.");
        }

        // Get the result of the migration process
        [HttpGet("result")]
        public IActionResult Result()
        {
            if (_system == null)
                return BadRequest("Migration system is not initialized.");

            var result = _system.Students.Select(s => new
            {
                s.Name,
                s.Id,
                Department = s.AcceptedDepartment?.Name ?? "None",
                s.Status,
                s.IsMigrationEnabled
            }).ToList();

            return Ok(result);
        }
    }


    public class MigrationInitRequest
    {
        public int Calls { get; set; }
        public List<DepartmentDto> Departments { get; set; }
        public List<StudentDto> Students { get; set; }
    }

    public class DepartmentDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
    }

    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public List<string> Choices { get; set; }
    }
}
