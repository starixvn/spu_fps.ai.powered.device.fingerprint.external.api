using Device.Fingerprint.ExternalAPI.DataAccess;
using Device.Fingerprint.ExternalAPI.Entities;
using Device.Fingerprint.ExternalAPI.Models;
using Device.Fingerprint.ExternalAPI.Services;
using Moq;

namespace Device.Fingerprint.ExternalAPI.Tests.Services
{
    public class SubscriberServiceTests
    {
        private readonly Mock<ISubscriberDataAccess> mockSubscriberDataAccess;
        private readonly SubscriberService subscriberService;

        public SubscriberServiceTests()
        {
            mockSubscriberDataAccess = new Mock<ISubscriberDataAccess>();
            subscriberService = new SubscriberService(mockSubscriberDataAccess.Object);
        }

        [Fact]
        public void GetSubscriberByName_ShouldReturnSubscriberModel_WhenSubscriberExists()
        {
            // Arrange
            var subscriberName = "TestSubscriber";
            var expectedSubscriber = new SubscriberModel
            {
                SubscriberId = 1,
                SubscriberName = subscriberName
            };

            mockSubscriberDataAccess.Setup(x => x.GetSubscriberByName(subscriberName))
                .Returns(new SubscriberEntity
                {
                    SubscriberId = 1,
                    SubscriberName = subscriberName
                });

            // Act
            var result = subscriberService.GetSubscriberByName(subscriberName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSubscriber.SubscriberId, result.SubscriberId);
            Assert.Equal(expectedSubscriber.SubscriberName, result.SubscriberName);
        }
    }
}
