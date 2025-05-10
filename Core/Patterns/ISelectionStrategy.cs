using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public interface ISelectionStrategy
    {
        Student SelectStudent(IEnumerable<Student> students);
    }
}
