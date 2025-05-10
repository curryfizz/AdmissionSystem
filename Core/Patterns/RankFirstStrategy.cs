using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class RankFirstStrategy : ISelectionStrategy
    {
        public Student SelectStudent(IEnumerable<Student> students)
            => students.OrderBy(s => s.Rank).FirstOrDefault();
    }
}
