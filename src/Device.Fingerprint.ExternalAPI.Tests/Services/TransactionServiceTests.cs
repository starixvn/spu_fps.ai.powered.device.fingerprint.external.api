using Device.Fingerprint.ExternalAPI.DataAccess;
using Device.Fingerprint.ExternalAPI.Entities;
using Device.Fingerprint.ExternalAPI.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.Fingerprint.ExternalAPI.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionDataAccess> mockTransactionDataAccess;
        private readonly TransactionService transactionService;

        public TransactionServiceTests()
        {
            mockTransactionDataAccess = new Mock<ITransactionDataAccess>();
            transactionService = new TransactionService(mockTransactionDataAccess.Object);
        }

        [Fact]
        public void GetTransactionHistoryByTransIdSubscriber_ShouldReturnTransactions()
        {
            // Arrange
            int subscriberId = 1;
            long transId = 1001;
            int limit = 10;
            var expectedTransactions = new List<Entities.TransactionHistoryEntity>
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
                    TransID = transId,
                    TransTime = DateTime.UtcNow,
                    AccountID = 1001,
                    UserName = "user1",
                    ActionResult = "Success",
                    RawFingerPrintID = 10,
                    RawUserAgentID = 20
                }
            };
            mockTransactionDataAccess
                .Setup(m => m.GetTransactionHistoryByTransIdSubscriber(subscriberId, transId, limit))
                .Returns(expectedTransactions);

            // Act
            var result = transactionService.GetTransactionHistoryByTransIdSubscriber(subscriberId, transId, limit);

            // Assert
            Assert.Equal(expectedTransactions, result);
        }

        [Fact]
        public void GetAssociationByAccountSubscriber_ShouldReturnAssociations()
        {
            // Arrange
            int subscriberId = 1;
            string account = "testAccount";
            var expectedDeviceAssociations = new List<AssociationDeviceEntity>
            {
                new AssociationDeviceEntity
                {
                    DeviceID = 2001,
                    Device = "Device123",
                    FirstSeenDate = DateTime.UtcNow.AddDays(-5),
                }
            };

            var expectedAccountAssociations = new List<AssociationAccountEntity>
            {
                new AssociationAccountEntity
                {
                    AccountID = 1001,
                    Account = "account",
                    FirstAssociationDate = DateTime.UtcNow.AddDays(-10),
                }
            };

            mockTransactionDataAccess
                .Setup(m => m.GetAssociationByAccountSubscriber(subscriberId, account))
                .Returns((expectedDeviceAssociations, expectedAccountAssociations));

            // Act
            var (deviceAssociations, accountAssociations) = transactionService.GetAssociationByAccountSubscriber(subscriberId, account);
            // Assert
            Assert.Equal(expectedDeviceAssociations, deviceAssociations);
        }

        [Fact]
        public void GetAssociationBySubscriberDevice_ShouldReturnAccountUsages()
        {
            // Arrange
            int subscriberId = 1;
            string deviceCode = "device123";
            DateTime fromDate = DateTime.Now.AddDays(-10);
            DateTime toDate = DateTime.Now;
            var expectedAccountUsages = new List<Entities.AssociationAccountUsageEntity>
            {
                new AssociationAccountUsageEntity
                {
                    LoginName = "account",
                    MaxUsageDate = DateTime.UtcNow,
                    MnUsageDate = DateTime.UtcNow.AddDays(-30),
                    UsageCount = 5
                }
            };
            mockTransactionDataAccess
                .Setup(m => m.GetAssociationBySubscriberDevice(subscriberId, deviceCode, fromDate, toDate))
                .Returns(expectedAccountUsages);
            // Act
            var result = transactionService.GetAssociationBySubscriberDevice(subscriberId, deviceCode, fromDate, toDate);
            // Assert
            Assert.Equal(expectedAccountUsages, result);
        }
    }
}
