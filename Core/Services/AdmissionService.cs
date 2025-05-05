using AdmissionSystem.Core.Factories;
using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using System.Linq;

namespace AdmissionSystem.Core.Services
{
    public class AdmissionService
    {
        private readonly AdmissionSystem.Core.Patterns.AdmissionSystem _admissionSystem;

        public AdmissionService(AdmissionSystem.Core.Patterns.AdmissionSystem admissionSystem)
        {
            _admissionSystem = admissionSystem;
        }

        public bool ApplyForSeat(string centerName, string studentName)
        {
            var center = _admissionSystem.Centers.FirstOrDefault(c => c.Name == centerName);
            if (center == null)
                return false;

            var room = center.GetAvailableRoom();
            if (room != null && room.AssignStudent())
            {
                return true;
            }

            return false;
        }

        public bool OpenNewRoom(string centerName)
        {
            var center = _admissionSystem.Centers.FirstOrDefault(c => c.Name == centerName);
            if (center == null)
                return false;

            // Use ActiveRooms or AllRooms as needed
            var newRoom = RoomFactory.CreateRoom($"R{center.AllRooms.Count + 1}", 10, center.Name);
            center.AddRoom(newRoom);
            return true;
        }

    }
}
