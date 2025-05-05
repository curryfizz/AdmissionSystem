namespace AdmissionSystem.Core.Models
{
    // Models/Room.cs
    public class Room
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
    }

}
