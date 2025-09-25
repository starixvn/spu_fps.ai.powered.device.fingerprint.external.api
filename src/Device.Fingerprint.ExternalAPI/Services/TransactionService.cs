using Device.Fingerprint.ExternalAPI.DataAccess;
using Device.Fingerprint.ExternalAPI.Entities;

namespace Device.Fingerprint.ExternalAPI.Services
{

    public interface ITransactionService
    {
        IEnumerable<TransactionHistoryEntity> GetTransactionHistoryByTransIdSubscriber(int subscriberId, long transId, int limit);
        (IEnumerable<AssociationDeviceEntity>, IEnumerable<AssociationAccountEntity>) GetAssociationByAccountSubscriber(int subscriberId, string account);
        IEnumerable<AssociationAccountUsageEntity> GetAssociationBySubscriberDevice(int subscriberId, string deviceCode, DateTime fromDate, DateTime toDate);

    }

    public class TransactionService : ITransactionService
    {
        private readonly ITransactionDataAccess transactionDataAccess;

        public TransactionService(ITransactionDataAccess transactionDataAccess)
        {
            this.transactionDataAccess = transactionDataAccess;
        }
        public IEnumerable<TransactionHistoryEntity> GetTransactionHistoryByTransIdSubscriber(int subscriberId, long transId, int limit)
        {
            return transactionDataAccess.GetTransactionHistoryByTransIdSubscriber(subscriberId, transId, limit);
        }

        public (IEnumerable<AssociationDeviceEntity>, IEnumerable<AssociationAccountEntity>) GetAssociationByAccountSubscriber(int subscriberId, string account)
        {
            return transactionDataAccess.GetAssociationByAccountSubscriber(subscriberId, account);
        }

        public IEnumerable<AssociationAccountUsageEntity> GetAssociationBySubscriberDevice(int subscriberId, string deviceCode, DateTime fromDate, DateTime toDate)
        {
            return transactionDataAccess.GetAssociationBySubscriberDevice(subscriberId, deviceCode, fromDate, toDate);
        }
    }
}
