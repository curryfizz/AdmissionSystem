using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class InitialAssignmentStrategy : ISeatAssignmentStrategy
    {
        public void AssignSeats(List<Student> students, List<Department> departments)
        {
            var ranked = students.OrderBy(s => s.Rank);
            foreach (var student in ranked)
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
