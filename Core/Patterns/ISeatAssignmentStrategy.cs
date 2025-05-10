using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public interface ISeatAssignmentStrategy
    {
        void AssignSeats(List<Student> students, List<Department> departments);
    }
}
