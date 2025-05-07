using AdmissionSystem.Core.Factories;
using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionSystem.API.Controllers
{
    [ApiController]
    [Route("api/admission")]
    public class AdmissionController : ControllerBase
    {
        private readonly AdmissionSystem.Core.Patterns.AdmissionSystem _admissionSystem;

        public AdmissionController()
        {
            _admissionSystem = AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance();
        }

        // POST: api/admission/init
        [HttpPost("init")]
        public IActionResult Init([FromBody] InitializeRequest request)
        {
            var center = new AdmissionCenter(request.CenterName, new UtilizationAwareStrategy());

            for (int i = 1; i <= request.RoomCount; i++)
            {
                // Create rooms with the RoomFactory
                var room = RoomFactory.CreateRoom($"R{i}", request.RoomCapacity, center.Name);
                center.AddRoom(room);
            }

            RoomFactory.CreateInitialRooms();
            _admissionSystem.RegisterCenter(center);

            return Ok("System Initialized");
        }

        // POST: api/admission/apply
        [HttpPost("apply")]
        public IActionResult Apply([FromBody] ApplyRequest request)
        {
            var center = _admissionSystem.Centers.FirstOrDefault();
            if (center == null)
                return BadRequest("No center registered.");

            var success = center.AssignSeatToStudent();
            return success ? Ok("Seat assigned.") : BadRequest("No seat available.");
        }

        // POST: api/admission/open-room
        [HttpPost("open-room")]
        public IActionResult OpenRoom([FromBody] OpenRoomRequest request)
        {
            var center = _admissionSystem.Centers.FirstOrDefault();
            if (center == null)
                return BadRequest("No center found.");

            // Check if the room already exists
            if (center.GetRooms().Any(r => r.Id == request.RoomId))
            {
                return BadRequest("Room already exists.");
            }

            // Create the room based on the provided request details
            var room = new Room(request.RoomId, request.RoomCapacity, center.Name);
            center.AddRoom(room);

            // Optionally, notify observers of the new room
            center.NotifyObservers(room);

            return Ok(new { Message = "Room opened successfully.", Room = room });
        }
    }

    // Example DTO classes for request bodies

    public class InitializeRequest
    {
        public string CenterName { get; set; }
        public int RoomCount { get; set; }
        public int RoomCapacity { get; set; }
    }

    public class ApplyRequest
    {
        public string StudentName { get; set; }
    }

    public class OpenRoomRequest
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public int RoomCapacity { get; set; }
    }
}
