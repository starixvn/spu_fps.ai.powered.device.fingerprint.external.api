using Device.Fingerprint.ExternalAPI.Services;
using Moq;

namespace Device.Fingerprint.ExternalAPI.Tests.Services
{
    public class RequestCounterServiceTests
    {
        private readonly Mock<IRedisService> _mockRedisService;
        private readonly RequestCounterService _requestCounterService;

        public RequestCounterServiceTests()
        {
            _mockRedisService = new Mock<IRedisService>();
            _requestCounterService = new RequestCounterService(_mockRedisService.Object);
        }

        [Fact]
        public async Task IncrementRequestCountAsync_ShouldCallHashIncrementAsync()
        {
            // Arrange
            string functionName = "TestFunction";
            string dateKey = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");

            // Act
            await _requestCounterService.IncrementRequestCountAsync(functionName);

            // Assert
            _mockRedisService.Verify(r => r.HashIncrementAsync(functionName, dateKey, 1), Times.Once);
        }
    }
}
