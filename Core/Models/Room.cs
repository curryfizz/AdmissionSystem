using AdmissionSystem.Core.Patterns;

namespace AdmissionSystem.Core.Models
{
    public class Room : IObserver
    {
        public string Id { get; }
        public int Capacity { get; }
        public int Occupied { get; private set; }
        public string CenterName { get; }

        public Room(string id, int capacity, string centerName)
        {
            Id = id;
            Capacity = capacity;
            Occupied = 0;
            CenterName = centerName;
        }

        public double Utilization => (double)Occupied / Capacity;

        public bool HasSpace => Occupied < Capacity;

        public bool AssignStudent()
        {
            if (HasSpace)
            {
                Occupied++;
                return true;
            }
            return false;
        }

        // Observer pattern: respond to updates from AdmissionCenter
        public void Update(IRoomSubject subject)
        {
            // Optional: log or respond to state changes if needed
            // For now, just a placeholder
            Console.WriteLine($"Room {Id} received update from {subject.GetType().Name}");
        }
    }
}
