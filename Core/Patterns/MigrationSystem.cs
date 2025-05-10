using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class MigrationSystem
    {
        public List<Department> Departments { get; } = new();
        public List<Student> Students { get; } = new();
        public int MaxCalls { get; }
        public int CurrentCall { get; private set; }

        public MigrationSystem(int maxCalls)
        {
            MaxCalls = maxCalls;
        }

        public void Initialize()
        {
            // Sort students by rank
            var sortedStudents = Students.OrderBy(s => s.Rank).ToList();

            // Assign departments based on student preferences and availability
            foreach (var student in sortedStudents)
            {
                foreach (var department in student.Choices)
                {
                    if (department.HasVacancy)
                    {
                        student.AcceptedDepartment = department;
                        department.AddStudent(student);
                        break;
                    }
                }
            }
        }

        public void RunNextCall()
        {
            if (CurrentCall >= MaxCalls) return;

            foreach (var student in Students)
            {
                if (student.AcceptedDepartment == null || !student.IsMigrationEnabled)
                    continue;

                var currentIndex = student.Choices.IndexOf(student.AcceptedDepartment);

                // Look for a better department (earlier in preferences)
                foreach (var dept in student.Choices.Take(currentIndex))
                {
                    if (dept.HasVacancy)
                    {
                        student.AcceptedDepartment.RemoveStudent(student);
                        student.AcceptedDepartment = dept;
                        dept.AddStudent(student);
                        break;
                    }
                }
            }

            CurrentCall++;
        }

        public void FinalizeStudent(int studentId)
        {
            var student = Students.FirstOrDefault(s => s.Id == studentId);
            student?.DisableMigration();
        }
    }
}
