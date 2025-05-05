using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public interface ISeatAllocationStrategy
    {
        bool AssignSeat(List<Room> rooms);
    }
}
