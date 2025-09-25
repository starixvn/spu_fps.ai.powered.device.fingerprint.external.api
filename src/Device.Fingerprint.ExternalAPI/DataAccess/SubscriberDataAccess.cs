using Device.Fingerprint.ExternalAPI.Criterias;
using Device.Fingerprint.ExternalAPI.Entities;
using Fanex.Data;
using Fanex.Data.Repository;

namespace Device.Fingerprint.ExternalAPI.DataAccess
{
    public interface ISubscriberDataAccess
    {
        SubscriberEntity GetSubscriberByName(string subscriberName);

        IEnumerable<SubscriberEntity> GetSubscriberList();
    }

    public class SubscriberDataAccess : ISubscriberDataAccess
    {
        private readonly IDynamicRepository repository;

        public SubscriberDataAccess()
        {
            repository = new DynamicRepository();
        }

        public IEnumerable<SubscriberEntity> GetSubscriberList()
        {
            var criteria = new GetSubscriberListCriteria();

            return repository.Fetch<SubscriberEntity>(criteria);
        }

        public SubscriberEntity GetSubscriberByName(string subscriberName)
        {
            var criteria = new GetSubscriberInfoByNameCriteria
            {
                SubscriberName = subscriberName
            };

            using var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey());
            return db.QueryEntity<SubscriberEntity>(criteria);
        }
    }

}
