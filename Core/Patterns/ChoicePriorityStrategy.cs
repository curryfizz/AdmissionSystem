using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class ChoicePriorityStrategy : ISelectionStrategy
    {
        private readonly Department _targetDepartment;

        public ChoicePriorityStrategy(Department targetDepartment)
        {
            _targetDepartment = targetDepartment;
        }

        public Student SelectStudent(IEnumerable<Student> students)
            => students.OrderBy(s => s.Choices.IndexOf(_targetDepartment))
                      .FirstOrDefault();
    }
}
