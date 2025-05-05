using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Factories
{
    // Factory/RoomFactory.cs
    public class RoomFactory
    {
        public static Room CreateRoom(string id, int capacity, string centerName)
        {
            return new Room(id, capacity, centerName);
        }
    }

}
