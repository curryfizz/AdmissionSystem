using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class MigrationSystem
    {
        private readonly ISelectionStrategy _selectionStrategy;
        private readonly ISeatAssignmentStrategy _assignmentStrategy;

        public List<Department> Departments { get; } = new();
        public List<Student> Students { get; } = new();
        public int MaxCalls { get; }
        public int CurrentCall { get; private set; }

        public MigrationSystem(int maxCalls, ISelectionStrategy selectionStrategy, ISeatAssignmentStrategy assignmentStrategy)
        {
            MaxCalls = maxCalls;
            _selectionStrategy = selectionStrategy;
            _assignmentStrategy = assignmentStrategy;
        }

        public void Initialize()
        {
            _assignmentStrategy.AssignSeats(Students, Departments);
        }

        public void RunNextCall()
        {
            if (CurrentCall >= MaxCalls) return;

            var studentToMigrate = _selectionStrategy.SelectStudent(Students.Where(s => s.IsMigrationEnabled).ToList());

            if (studentToMigrate != null)
            {
                foreach (var department in studentToMigrate.Choices)
                {
                    if (department.HasVacancy)
                    {
                        studentToMigrate.AcceptedDepartment?.RemoveStudent(studentToMigrate);

                        studentToMigrate.AcceptedDepartment = department;
                        department.AddStudent(studentToMigrate);
                        break;
                    }
                }
            }

            CurrentCall++;
        }

        public void FinalizeStudent(int studentId)
        {
            var student = Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                student.DisableMigration(); // Disable further migration for the student
            }
        }
    }
}
