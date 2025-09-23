using Device.Fingerprint.ExternalAPI.Controllers;
using Device.Fingerprint.ExternalAPI.Entities;
using Device.Fingerprint.ExternalAPI.Models;
using Device.Fingerprint.ExternalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Device.Fingerprint.ExternalAPI.Tests.Controllers
{
    public class CustomerClassControllerTests
    {
        private readonly Mock<ISubscriberService> _mockSubscriberService;
        private readonly Mock<IRiskLevelService> _mockRiskLevelService;
        private readonly CustomerClassController _controller;

        public CustomerClassControllerTests()
        {
            _mockSubscriberService = new Mock<ISubscriberService>();
            _mockRiskLevelService = new Mock<IRiskLevelService>();
            _controller = new CustomerClassController(_mockSubscriberService.Object, _mockRiskLevelService.Object);
        }

        [Fact]
        public void GetRiskLevelBySubscriber_ShouldReturnRiskLevels()
        {
            // Arrange
            string subscriberName = "TestSubscriber";
            long lastId = 0;
            var subscriber = new SubscriberModel { SubscriberId = 1, SubscriberName = subscriberName };
            var expectedRiskLevels = new List<RiskLevelEntity>
            {
                new() { Id = 1, RegisterName = "Account1", RiskLevel = "Low" },
                new() { Id = 2, RegisterName = "Account2", RiskLevel = "High" }
            };
            _mockSubscriberService
                .Setup(x => x.GetSubscriberByName(subscriberName))
                .Returns(subscriber);
            _mockRiskLevelService
                .Setup(x => x.GetCustomerClassRiskLevel(subscriber.SubscriberId, lastId, 1000))
                .Returns(expectedRiskLevels);
            // Act
            var result = _controller.GetRiskLevelBySubscriber(subscriberName, lastId);
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var riskLevelList = Assert.IsType<CustomerClassRiskLevelList>(okResult.Value);
            Assert.Equal(System.Net.HttpStatusCode.OK, riskLevelList.StatusCode);
            Assert.NotNull(riskLevelList.ErrorMessage);
            Assert.NotNull(riskLevelList.Trans);
        }

        [Fact]
        public void GetRiskLevelBySubscriber_ShouldReturnNotFound_WhenSubscriberIsNull()
        {
            // Arrange
            string subscriberName = "NonExistentSubscriber";
            long lastId = 0;
            _mockSubscriberService
                .Setup(x => x.GetSubscriberByName(subscriberName))
                .Returns(new SubscriberModel());

            // Act
            var result = _controller.GetRiskLevelBySubscriber(subscriberName, lastId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var riskLevelList = Assert.IsType<CustomerClassRiskLevelList>(okResult.Value);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, riskLevelList.StatusCode);
            Assert.Equal("Subscriber cannot be null", riskLevelList.ErrorMessage);
            Assert.Null(riskLevelList.Trans);
        }
    }
}
