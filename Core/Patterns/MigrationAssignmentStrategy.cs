using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class MigrationAssignmentStrategy : ISeatAssignmentStrategy
    {
        public void AssignSeats(List<Student> students, List<Department> departments)
        {
            foreach (var student in students.Where(s =>
                s.Status == AdmissionStatus.Accepted && s.IsMigrationEnabled))
            {
                var current = student.AcceptedDepartment;

                foreach (var preferred in student.Choices)
                {
                    if (preferred == current)
                        break; 

                    if (preferred.HasVacancy)
                    {
                        student.DeclineOffer();
                        student.SetTentativeOffer(preferred);
                        student.AcceptOffer(); 
                        break;
                    }
                }
            }

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
