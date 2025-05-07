using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public class UtilizationAwareStrategy : ISeatAllocationStrategy
    {
        public Room? SelectRoom(List<Room> rooms)
        {
            return rooms
                .Where(r => r.HasSpace)
                .OrderBy(r => r.Utilization)
                .FirstOrDefault();
        }
    }
}
