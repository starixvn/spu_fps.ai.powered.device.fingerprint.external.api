using Device.Fingerprint.ExternalAPI.Criterias;
using Device.Fingerprint.ExternalAPI.Entities;
using Fanex.Data.Repository;

namespace Device.Fingerprint.ExternalAPI.DataAccess
{
    public interface ITransactionDataAccess
    {
        IEnumerable<TransactionHistoryEntity> GetTransactionHistoryByTransIdSubscriber(int subscriberId, long transId, int limit);

        (IEnumerable<AssociationDeviceEntity>, IEnumerable<AssociationAccountEntity>) GetAssociationByAccountSubscriber(int subscriberId, string account);

        IEnumerable<AssociationAccountUsageEntity> GetAssociationBySubscriberDevice(int subscriberId, string deviceCode, DateTime fromDate, DateTime toDate);
    }

    public class TransactionDataAccess : ITransactionDataAccess
    {
        private readonly IDynamicRepository repository;

        public TransactionDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<TransactionHistoryEntity> GetTransactionHistoryByTransIdSubscriber(int subscriberId, long transId, int limit)
        {
            var criteria = new GetTransactionBySubscriberCriteria
            {
                TransId = transId,
                Limit = limit,
                SubscriberId = subscriberId,
            };

            return repository.Fetch<TransactionHistoryEntity>(criteria);
        }

        public (IEnumerable<AssociationDeviceEntity>, IEnumerable<AssociationAccountEntity>) GetAssociationByAccountSubscriber(int subscriberId, string account)
        {
            var criteria = new GetAssociationBySubscriberCriteria
            {
                SubscriberId = subscriberId,
                Account = account
            };

            return repository.FetchMultiple<AssociationDeviceEntity, AssociationAccountEntity>(criteria);
        }

        public IEnumerable<AssociationAccountUsageEntity> GetAssociationBySubscriberDevice(int subscriberId, string deviceCode, DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetAssociationBySubscriberDeviceCriteria
            {
                DeviceCode = deviceCode,
                FromDate = fromDate,
                ToDate = toDate,
                SubscriberId = subscriberId,
            };

            return repository.Fetch<AssociationAccountUsageEntity>(criteria);
        }

    }
}
