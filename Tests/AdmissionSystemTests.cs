using AdmissionSystem.Core.Models;
using AdmissionSystem.Core.Patterns;
using Xunit;

namespace AdmissionSystem.Tests
{
    public class AdmissionSystemTests
    {
        [Fact]
        public void GetInstance_ShouldReturnSingletonInstance()
        {
            // Act
            var instance1 = AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance();
            var instance2 = AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance();

            // Assert
            Assert.Same(instance1, instance2);
        }

        [Fact]
        public void RegisterCenter_ShouldAddCenterToSystem()
        {
            // Arrange
            var system = AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance();
            var center = new AdmissionCenter("Test Center", new UtilizationAwareStrategy());

            // Act
            system.RegisterCenter(center);

            // Assert
            Assert.Contains(center, system.Centers);
        }

        [Fact]
        public void GetCenterById_ShouldReturnCorrectCenter()
        {
            // Arrange
            var system = AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance();
            var center = new AdmissionCenter("Test Center", new UtilizationAwareStrategy());
            system.RegisterCenter(center);

            // Act
            var retrievedCenter = system.GetCenterById(center.CenterId);

            // Assert
            Assert.Equal(center, retrievedCenter);
        }

        [Fact]
        public void GetCenterById_ShouldReturnNull_WhenCenterNotFound()
        {
            // Arrange
            var system = AdmissionSystem.Core.Patterns.AdmissionSystem.GetInstance();

            // Act
            var retrievedCenter = system.GetCenterById(999);

            // Assert
            Assert.Null(retrievedCenter);
        }
    }
}
