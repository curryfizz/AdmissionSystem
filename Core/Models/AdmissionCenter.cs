using AdmissionSystem.Core.Patterns;

namespace AdmissionSystem.Core.Models
{
    public class AdmissionCenter
    {
        public string Name { get; }
        public List<Room> AllRooms { get; }
        public List<Room> ActiveRooms { get; }

        private readonly ISeatAllocationStrategy _strategy;
        private readonly int MinActiveRooms;
        private readonly double UtilThreshold;

        public AdmissionCenter(string name, ISeatAllocationStrategy strategy, int minRooms = 3, double threshold = 0.85)
        {
            Name = name;
            _strategy = strategy;
            AllRooms = new();
            ActiveRooms = new();
            MinActiveRooms = minRooms;
            UtilThreshold = threshold;
        }

        public void AddRoom(Room room) => AllRooms.Add(room);

        public void ActivateInitialRooms()
        {
            var available = AllRooms.Take(MinActiveRooms).ToList();
            ActiveRooms.AddRange(available);
        }

        public bool AssignSeatToStudent()
        {
            var assigned = _strategy.AssignSeat(ActiveRooms);
            if (!assigned)
                TryAddMoreRoom();

            return assigned || _strategy.AssignSeat(ActiveRooms);
        }

        public bool AddNewRoomManually()
        {
            var next = AllRooms.Except(ActiveRooms).FirstOrDefault();
            if (next != null)
            {
                ActiveRooms.Add(next);
                return true;
            }
            return false;
        }

        public Room GetAvailableRoom()
        {
            return ActiveRooms.FirstOrDefault(room => room.HasSpace);
        }

        // Define the TryAddMoreRoom method
        private void TryAddMoreRoom()
        {
            var avgUtil = ActiveRooms.Average(r => r.Utilization); // Calculate average utilization of active rooms
            if (avgUtil >= UtilThreshold && AllRooms.Count > ActiveRooms.Count) // Check if the threshold is met
            {
                var nextRoom = AllRooms.Except(ActiveRooms).FirstOrDefault(); // Get the next available room
                if (nextRoom != null)
                {
                    ActiveRooms.Add(nextRoom); // Add the room to active rooms
                }
            }
        }
    }
}
