using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using AdmissionSystem.Core.Factories;
using System.Linq;
using Xunit;

namespace AdmissionSystem.Tests
{
    public class AdmissionCenterTests
    {
        private AdmissionCenter SetupTestCenter(int totalRooms = 5, int roomCapacity = 2)
        {
            var center = new AdmissionCenter("TestCenter", new UtilizationAwareStrategy());
            for (int i = 1; i <= totalRooms; i++)
            {
                center.AddRoom(RoomFactory.CreateRoom($"T{i}", roomCapacity, "TestCenter"));
            }
            center.ActivateInitialRooms();
            return center;
        }

        [Fact]
        public void Should_Assign_Seat_When_Room_Has_Space()
        {
            // Arrange
            var center = SetupTestCenter();

            // Act
            var result = center.AssignSeatToStudent();

            // Assert
            Assert.True(result);
            Assert.Equal(1, center.ActiveRooms.Sum(r => r.Occupied));
        }

        [Fact]
        public void Should_Not_Assign_Seat_When_All_Rooms_Full()
        {
            // Arrange
            var center = SetupTestCenter(totalRooms: 1, roomCapacity: 1);
            center.AssignSeatToStudent(); // Fill the only seat

            // Act
            var result = center.AssignSeatToStudent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_Open_New_Room_When_Utilization_Threshold_Exceeded()
        {
            // Arrange
            var center = SetupTestCenter(totalRooms: 5, roomCapacity: 2);

            // Fill 3 rooms (capacity = 6)
            for (int i = 0; i < 6; i++)
            {
                center.AssignSeatToStudent();
            }

            var activeBefore = center.ActiveRooms.Count;

            // Act: This will exceed utilization and trigger new room
            center.AssignSeatToStudent();
            var activeAfter = center.ActiveRooms.Count;

            // Assert
            Assert.True(activeAfter > activeBefore);
        }

        [Fact]
        public void Should_Add_New_Room_Manually_When_Available()
        {
            // Arrange
            var center = SetupTestCenter();

            var initialActive = center.ActiveRooms.Count;

            // Act
            var result = center.AddNewRoomManually();

            // Assert
            Assert.True(result);
            Assert.Equal(initialActive + 1, center.ActiveRooms.Count);
        }

        [Fact]
        public void Should_Not_Add_New_Room_Manually_If_None_Left()
        {
            // Arrange
            var center = SetupTestCenter(totalRooms: 3);
            while (center.AddNewRoomManually()) { }

            // Act
            var result = center.AddNewRoomManually();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Room_Utilization_Should_Be_Calculated_Correctly()
        {
            // Arrange
            var room = RoomFactory.CreateRoom("R1", 4, "Test");
            room.AssignStudent(); room.AssignStudent(); // 2/4 = 0.5

            // Act
            var utilization = room.Utilization;

            // Assert
            Assert.Equal(0.5, utilization, 1); // 1 digit precision
        }

        [Fact]
        public void Room_Should_Not_Assign_If_Full()
        {
            // Arrange
            var room = RoomFactory.CreateRoom("R2", 1, "Test");
            room.AssignStudent(); // Now full

            // Act
            var result = room.AssignStudent();

            // Assert
            Assert.False(result);
        }
    }
}
