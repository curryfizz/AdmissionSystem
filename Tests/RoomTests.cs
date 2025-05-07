using AdmissionSystem.Core.Models;
using Xunit;

namespace AdmissionSystem.Tests
{
    public class RoomTests
    {
        [Fact]
        public void AssignStudent_ShouldIncreaseOccupiedCount_WhenHasSpace()
        {
            // Arrange
            var room = new Room("R1", 10, "CenterAlpha");

            // Act
            var success = room.AssignStudent();

            // Assert
            Assert.True(success);
            Assert.Equal(1, room.Occupied);
        }

        [Fact]
        public void AssignStudent_ShouldNotIncreaseOccupiedCount_WhenNoSpace()
        {
            // Arrange
            var room = new Room("R1", 1, "CenterAlpha");
            room.AssignStudent(); // Occupied = 1

            // Act
            var success = room.AssignStudent(); // No space left

            // Assert
            Assert.False(success);
            Assert.Equal(1, room.Occupied);
        }

        [Fact]
        public void Utilization_ShouldReturnCorrectUtilization()
        {
            // Arrange
            var room = new Room("R1", 10, "CenterAlpha");
            room.AssignStudent(); // Occupied = 1

            // Act
            var utilization = room.Utilization;

            // Assert
            Assert.Equal(0.1, utilization);
        }
    }
}
