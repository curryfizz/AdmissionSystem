using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using Xunit;

namespace AdmissionSystem.Tests
{
    public class UtilizationAwareStrategyTests
    {
        [Fact]
        public void SelectRoom_ShouldSelectRoomWithLeastUtilization()
        {
            // Arrange
            var room1 = new Room("R1", 10, "Test Center");
            room1.AssignStudent();
            var room2 = new Room("R2", 10, "Test Center");
            room2.AssignStudent();
            room2.AssignStudent();

            var strategy = new UtilizationAwareStrategy();

            // Act
            var selectedRoom = strategy.SelectRoom(new List<Room> { room1, room2 });

            // Assert
            Assert.Equal(room1, selectedRoom); // room1 should have the least utilization
        }

        [Fact]
        public void SelectRoom_ShouldReturnNull_WhenNoRoomHasSpace()
        {
            // Arrange
            var room1 = new Room("R1", 1, "Test Center");
            room1.AssignStudent(); // No space

            var strategy = new UtilizationAwareStrategy();

            // Act
            var selectedRoom = strategy.SelectRoom(new List<Room> { room1 });

            // Assert
            Assert.Null(selectedRoom); // No room available
        }
    }
}
