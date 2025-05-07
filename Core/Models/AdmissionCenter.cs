using AdmissionSystem.Core.Factories;
using AdmissionSystem.Core.Patterns;

namespace AdmissionSystem.Core.Models
{
    public class AdmissionCenter : IRoomSubject
    {
        public int CenterId { get; set; }
        public string Name { get; }

        private readonly ISeatAllocationStrategy _strategy;
        private readonly List<IObserver> _observers = new();
        private readonly List<Room> _rooms = new();

        public AdmissionCenter(string name, ISeatAllocationStrategy strategy)
        {
            Name = name;
            _strategy = strategy;
        }

        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }

        public bool AssignSeatToStudent()
        {
            var room = _strategy.SelectRoom(_rooms);
            if (room == null || !room.HasSpace)
                return false;

            var assigned = room.AssignStudent();
            if (assigned)
                NotifyObservers(room);
            return assigned;
        }

        public IReadOnlyList<Room> GetRooms() => _rooms;

        // Observer pattern
        public void Attach(IObserver observer) => _observers.Add(observer);
        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void NotifyObservers(Room room)
        {
            foreach (var observer in _observers)
            {
                observer.Update(this); // Update based on subject, not individual room
            }
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
