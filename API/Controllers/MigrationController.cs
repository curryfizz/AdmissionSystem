using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.API.Controllers
{
    [ApiController]
    [Route("api/migration")]
    public class MigrationController : ControllerBase
    {
        private static MigrationSystem _system = new(3);

        [HttpPost("init")]
        public IActionResult Init([FromBody] MigrationInitRequest request)
        {
            _system = new MigrationSystem(request.Calls);

            var departments = request.Departments
                .Select(d => new Department(d.Name, d.Capacity)).ToList();

            var students = request.Students
                .Select(s => new Student(s.Id, s.Name, s.Rank,
                    s.Choices.Select(c => departments.First(d => d.Name == c)).ToList())).ToList();

            _system.Departments.AddRange(departments);
            _system.Students.AddRange(students);

            _system.Initialize();

            return Ok("Migration system initialized.");
        }

        [HttpPost("call")]
        public IActionResult Call()
        {
            _system.RunNextCall();
            return Ok("Migration round completed.");
        }

        [HttpPost("finalize/{studentId}")]
        public IActionResult Finalize(int studentId)
        {
            _system.FinalizeStudent(studentId);
            return Ok("Migration turned off for student.");
        }

        [HttpGet("result")]
        public IActionResult Result()
        {
            return Ok(_system.Students.Select(s => new
            {
                s.Name,
                s.Id,
                Department = s.AcceptedDepartment?.Name ?? "None",
                s.Status,
                s.IsMigrationEnabled
            }));
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

