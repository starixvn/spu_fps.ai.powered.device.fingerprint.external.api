using Device.Fingerprint.ExternalAPI.DataAccess;
using Device.Fingerprint.ExternalAPI.Entities;
using Device.Fingerprint.ExternalAPI.Services;
using Moq;

namespace Device.Fingerprint.ExternalAPI.Tests.Services
{
    public class RiskLevelServiceTests
    {
        private readonly Mock<IRiskLevelDataAccess> _mockRiskLevelDataAccess;
        private readonly RiskLevelService _riskLevelService;

        public RiskLevelServiceTests()
        {
            _mockRiskLevelDataAccess = new Mock<IRiskLevelDataAccess>();
            _riskLevelService = new RiskLevelService(_mockRiskLevelDataAccess.Object);
        }

        [Fact]
        public void GetCustomerClassRiskLevel_ShouldReturnRiskLevels()
        {
            // Arrange
            int subscriberId = 1;
            long lastId = 0;
            int batchSize = 10;
            var expectedRiskLevels = new List<RiskLevelEntity>
            {
                new() { Id = 1, RiskLevel = "Low" },
                new() { Id = 2, RiskLevel = "High" }
            };
            _mockRiskLevelDataAccess
                .Setup(x => x.GetCustomerClassRiskLevel(subscriberId, lastId, batchSize))
                .Returns(expectedRiskLevels);

            // Act
            var result = _riskLevelService.GetCustomerClassRiskLevel(subscriberId, lastId, batchSize);

            // Assert
            Assert.Equal(expectedRiskLevels, result);
        }

        [Fact]
        public void GetCustomerClassRiskLevelByAccount_ShouldReturnRiskLevelsByAccount()
        {
            // Arrange
            string accountIDs = "123,456";
            var expectedRiskLevelsByAccount = new List<RiskLevelByAccountEntity>
            {
                new() { AccountID = 123, RiskLevel = "Medium" },
                new() { AccountID = 456, RiskLevel = "Low" }
            };

            _mockRiskLevelDataAccess
                .Setup(x => x.GetCustomerClassRiskLevelByAccount(accountIDs))
                .Returns(expectedRiskLevelsByAccount);

            // Act
            var result = _riskLevelService.GetCustomerClassRiskLevelByAccount(accountIDs);

            // Assert
            Assert.Equal(expectedRiskLevelsByAccount, result);
        }
    }
}
