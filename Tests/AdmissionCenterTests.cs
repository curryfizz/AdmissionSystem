using Moq;
using Xunit;
using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;

namespace AdmissionSystem.Tests
{
    public class AdmissionCenterTests
    {
        private readonly AdmissionCenter _center;

        public AdmissionCenterTests()
        {
            var strategy = new Mock<ISeatAllocationStrategy>();
            _center = new AdmissionCenter("Test Center", strategy.Object);
        }

        [Fact]
        public void AddRoom_ShouldAddRoomToCenter()
        {
            // Arrange
            var room = new Room("R1", 10, "Test Center");

            // Act
            _center.AddRoom(room);

            // Assert
            Assert.Contains(room, _center.GetRooms());
        }

        [Fact]
        public void AssignSeatToStudent_ShouldAssignSeat_WhenRoomHasSpace()
        {
            // Arrange
            var room = new Room("R1", 10, "Test Center");
            var strategy = new Mock<ISeatAllocationStrategy>();

            // Mocking the strategy to return the room when selecting
            strategy.Setup(s => s.SelectRoom(It.IsAny<List<Room>>())).Returns(room);

            // Create the AdmissionCenter with the mocked strategy
            var center = new AdmissionCenter("Test Center", strategy.Object);

            // Add the room to the center
            center.AddRoom(room);

            // Mock an observer and attach it to the center
            var observer = new Mock<IObserver>();
            center.Attach(observer.Object);

            // Act
            var success = center.AssignSeatToStudent();

            // Assert
            Assert.True(success);  // Ensure the seat assignment is successful
            Assert.Equal(1, room.Occupied);  // Ensure the room's Occupied property is incremented

            // Verify if observers are notified with the correct subject (AdmissionCenter)
            observer.Verify(o => o.Update(It.IsAny<IRoomSubject>()), Times.Once);
        }

        [Fact]
        public void AssignSeatToStudent_ShouldNotAssignSeat_WhenNoRoomAvailable()
        {
            // Arrange
            var strategy = new Mock<ISeatAllocationStrategy>();
            strategy.Setup(s => s.SelectRoom(It.IsAny<List<Room>>())).Returns((Room)null);
            _center.Attach(new Mock<IObserver>().Object);

            // Act
            var success = _center.AssignSeatToStudent();

            // Assert
            Assert.False(success);
        }

        [Fact]
        public void NotifyObservers_ShouldNotifyAllObservers()
        {
            // Arrange
            var room = new Room("R1", 10, "Test Center");
            var observerMock = new Mock<IObserver>();
            _center.Attach(observerMock.Object);

            // Act
            _center.NotifyObservers(room);

            // Assert
            observerMock.Verify(o => o.Update(_center), Times.Once);
        }
    }
}
