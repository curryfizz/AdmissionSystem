using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class MigrationAssignmentStrategy : ISeatAssignmentStrategy
    {
        public void AssignSeats(List<Student> students, List<Department> departments)
        {
            // Migrate accepted students who want better options
            foreach (var student in students.Where(s =>
                s.Status == AdmissionStatus.Accepted && s.IsMigrationEnabled))
            {
                var current = student.AcceptedDepartment;

                foreach (var preferred in student.Choices)
                {
                    if (preferred == current)
                        break; // stop trying after current choice

                    if (preferred.HasVacancy)
                    {
                        student.DeclineOffer(); // Frees up current seat
                        student.SetTentativeOffer(preferred);
                        student.AcceptOffer(); // Takes new dept
                        break;
                    }
                }

            }

            // Try to place remaining pending students
            foreach (var student in students.Where(s => s.Status == AdmissionStatus.Pending))
            {
                foreach (var department in student.Choices)
                {
                    if (department.HasVacancy)
                    {
                        student.SetTentativeOffer(department);
                        student.AcceptOffer(); 
                        break;
                    }
                }
            }
        }
    }
}
