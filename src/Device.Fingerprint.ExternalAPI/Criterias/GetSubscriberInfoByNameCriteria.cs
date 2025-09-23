using Fanex.Data;
using Fanex.Data.Repository;

namespace Device.Fingerprint.ExternalAPI.Criterias
{
    public class GetSubscriberInfoByNameCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_SubscriberName", AllowNull = true)]
        public required string SubscriberName { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_AD_Subscriber_GetInfoBySubscriberName";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
