using Fanex.Data.Repository;

namespace Device.Fingerprint.ExternalAPI.Criterias
{
    public class GetSubscriberListCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "FPS_FP_Core_SubscriberList_Get";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
