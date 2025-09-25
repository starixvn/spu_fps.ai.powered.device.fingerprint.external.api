using Device.Fingerprint.ExternalAPI.Criterias;
using Device.Fingerprint.ExternalAPI.Entities;
using Fanex.Data.Repository;

namespace Device.Fingerprint.ExternalAPI.DataAccess
{
    public interface IRiskLevelDataAccess
    {
        IEnumerable<RiskLevelEntity> GetCustomerClassRiskLevel(int subscriberId, long lastId, int batchSize);
        IEnumerable<RiskLevelByAccountEntity> GetCustomerClassRiskLevelByAccount(string accountIDs);
    }

    public class RiskLevelDataAccess : IRiskLevelDataAccess
    {
        private readonly IDynamicRepository repository;

        public RiskLevelDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<RiskLevelEntity> GetCustomerClassRiskLevel(int subscriberId, long lastId, int batchSize)
        {
            var criteria = new GetLatestRiskLevelCriteria
            {
                SubscriberId = subscriberId,
                LastId = lastId,
                BatchSize = batchSize
            };

            return repository.Fetch<RiskLevelEntity>(criteria);
        }

        public IEnumerable<RiskLevelByAccountEntity> GetCustomerClassRiskLevelByAccount(string accountIDs)
        {
            var criteria = new GetRiskLevelByAccountCriteria
            {
                AccountIDs = accountIDs
            };

            return repository.Fetch<RiskLevelByAccountEntity>(criteria);
        }
    }

}
