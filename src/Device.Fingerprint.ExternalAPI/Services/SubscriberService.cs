using Device.Fingerprint.ExternalAPI.DataAccess;
using Device.Fingerprint.ExternalAPI.Models;

namespace Device.Fingerprint.ExternalAPI.Services
{
    public interface ISubscriberService
    {
        SubscriberModel GetSubscriberByName(string subscriberName);
    }

    public class SubscriberService : ISubscriberService
    {

        private readonly ISubscriberDataAccess subscriberDataAccess;

        public SubscriberService(ISubscriberDataAccess subscriberDataAccess)
        {
            this.subscriberDataAccess = subscriberDataAccess;
        }

        public SubscriberModel GetSubscriberByName(string subscriberName)
        {
            var subcribers = subscriberDataAccess.GetSubscriberByName(subscriberName);

            if (subcribers != null)
            {
                return new SubscriberModel
                {
                    SubscriberId = subcribers.SubscriberId,
                    SubscriberName = subcribers.SubscriberName
                };
            }

            return new();
        }
    }
}
