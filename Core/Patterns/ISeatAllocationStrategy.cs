using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    public interface ISeatAllocationStrategy
    {
        Room? SelectRoom(List<Room> rooms);
    }
}
