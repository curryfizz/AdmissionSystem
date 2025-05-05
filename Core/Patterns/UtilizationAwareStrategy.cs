using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class UtilizationAwareStrategy : ISeatAllocationStrategy
    {
        public bool AssignSeat(List<Room> rooms)
        {
            var shuffled = rooms.Where(r => r.HasSpace).OrderBy(r => Guid.NewGuid());
            foreach (var room in shuffled)
                if (room.AssignStudent())
                    return true;
            return false;
        }
    }
}
