using AdmissionSystem.Core.Factories;
using AdmissionSystem.Core.Models;
using Xunit;

namespace AdmissionSystem.Tests
{
    public class RoomFactoryTests
    {
        [Fact]
        public void CreateRoom_ShouldReturnRoomWithCorrectValues()
        {
            // Arrange
            var roomId = "R1";
            var capacity = 10;
            var centerName = "CenterAlpha";

            // Act
            var room = RoomFactory.CreateRoom(roomId, capacity, centerName);

            // Assert
            Assert.NotNull(room);
            Assert.Equal(roomId, room.Id);
            Assert.Equal(capacity, room.Capacity);
            Assert.Equal(centerName, room.CenterName);
        }

        [Fact]
        public void CreateInitialRooms_ShouldReturnListWithCorrectRooms()
        {
            // Act
            var rooms = RoomFactory.CreateInitialRooms();

            // Assert
            Assert.Equal(3, rooms.Count);
            Assert.Contains(rooms, room => room.Id == "A1" && room.Capacity == 5);
            Assert.Contains(rooms, room => room.Id == "B1" && room.Capacity == 3);
            Assert.Contains(rooms, room => room.Id == "C1" && room.Capacity == 4);
        }
    }
}
