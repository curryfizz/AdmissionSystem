using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Factories
{
    public class RoomFactory
    {
        public static List<Room> CreateInitialRooms()
        {
            // You can change this logic to fetch from config/db if needed
            return new List<Room>
            {
                new Room("A1", 5, "CenterAlpha"),
                new Room("B1", 3, "CenterAlpha"),
                new Room("C1", 4, "CenterAlpha")
            };
        }

        public static Room CreateRoom(string id, int capacity, string centerName)
        {
            return new Room(id, capacity, centerName);
        }
    }
}
