using Device.Fingerprint.ExternalAPI.Controllers;
using Device.Fingerprint.ExternalAPI.Entities;
using Device.Fingerprint.ExternalAPI.Models;
using Device.Fingerprint.ExternalAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.Fingerprint.ExternalAPI.Tests.Controllers
{
    public class TransactionControllerTests
    {
        private readonly Mock<ISubscriberService> _mockSubscriberService;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<IRiskLevelService> _mockRiskLevelService;
        private readonly Mock<IAppSettings> _mockAppSettings;
        private readonly TransactionController _controller;

        public TransactionControllerTests()
        {
            _mockSubscriberService = new Mock<ISubscriberService>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mockRiskLevelService = new Mock<IRiskLevelService>();
            _mockAppSettings = new Mock<IAppSettings>();
            _controller = new TransactionController(
                _mockSubscriberService.Object,
                _mockTransactionService.Object,
                _mockRiskLevelService.Object,
                _mockAppSettings.Object);
        }

        [Fact]
        public void GetTransactionHistory_ReturnsOkResult_WithTransactionHistoryList()
        {
            // Arrange
            string subscriberName = "TestSubscriber";
            long trackingNumber = 123456789;
            var mockSubscriber = new SubscriberModel { SubscriberId = 1, SubscriberName = subscriberName };
            var mockTransactions = new List<TransactionHistoryEntity>
            {
                new TransactionHistoryEntity
                {
                    Action = "Login",
                    Browser = "Chrome",
                    UrlDetails = "example.com",
                    FirstDeviceCode = "Device123",
                    IP = "1.2.3.4",
                    OS = "Windows",
                    RobotTracking = "None",
                    TransID = trackingNumber,
                    TransTime = DateTime.UtcNow,
                    AccountID = 1001,
                    UserName = "user1",
                    ActionResult = "Success",
                    RawFingerPrintID = 10,
                    RawUserAgentID = 20
                }
            };
            var mockRiskLevels = new List<RiskLevelByAccountEntity>
            {
                new RiskLevelByAccountEntity
                {
                    AccountID = 1001,
                    RiskLevel = "Low"
                }
            };
            _mockSubscriberService.Setup(s => s.GetSubscriberByName(subscriberName)).Returns(mockSubscriber);
            _mockTransactionService.Setup(t => t.GetTransactionHistoryByTransIdSubscriber(mockSubscriber.SubscriberId, trackingNumber, 1000)).Returns(mockTransactions);
            _mockRiskLevelService.Setup(r => r.GetCustomerClassRiskLevelByAccount("1001")).Returns(mockRiskLevels);
            // Act
            var result = _controller.GetTransactionHistory(subscriberName, trackingNumber);
            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAssociationByAccount_ReturnsOkResult_WithAssociationList()
        {
            // Arrange
            string subscriberName = "TestSubscriber";
            string accountId = "1001";
            var mockSubscriber = new SubscriberModel { SubscriberId = 1, SubscriberName = subscriberName };
            var mockAccountAssociations = new List<AssociationAccountEntity>
            {
                new AssociationAccountEntity
                {
                    AccountID = 1001,
                    Account = "account",
                    FirstAssociationDate = DateTime.UtcNow.AddDays(-10),
                }
            };
            var mockDeviceAssociations = new List<AssociationDeviceEntity>
            {
                new AssociationDeviceEntity
                {
                    DeviceID = 2001,
                    Device = "Device123",
                    FirstSeenDate = DateTime.UtcNow.AddDays(-5),
                }
            };

            _mockSubscriberService.Setup(s => s.GetSubscriberByName(subscriberName)).Returns(mockSubscriber);
            _mockTransactionService.Setup(t => t.GetAssociationByAccountSubscriber(mockSubscriber.SubscriberId, accountId)).Returns((mockDeviceAssociations, mockAccountAssociations));
            // Act
            var result = _controller.GetAssociationByAccount(subscriberName, accountId);
            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAssociationByDevice_ReturnsOkResult_WithAssociationList()
        {
            // Arrange
            string subscriberName = "TestSubscriber";
            string deviceCode = "Device123";
            DateTime fromDate = DateTime.UtcNow.AddDays(-60);
            DateTime toDate = DateTime.UtcNow;
            var mockSubscriber = new SubscriberModel { SubscriberId = 1, SubscriberName = subscriberName };
            var mockDeviceUsageAssociations = new List<AssociationAccountUsageEntity>
            {
                new AssociationAccountUsageEntity
                {
                    LoginName = "account",
                    MaxUsageDate = DateTime.UtcNow,
                    MnUsageDate = DateTime.UtcNow.AddDays(-30),
                    UsageCount = 5
                }
            };
            _mockSubscriberService.Setup(s => s.GetSubscriberByName(subscriberName)).Returns(mockSubscriber);
            _mockTransactionService.Setup(t => t.GetAssociationBySubscriberDevice(mockSubscriber.SubscriberId, deviceCode, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(mockDeviceUsageAssociations);
            // Act
            var result = _controller.GetAssociationByDevice(subscriberName, deviceCode, fromDate, toDate);
            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Mvc.OkObjectResult>(result.Result);
        }
    }
}
