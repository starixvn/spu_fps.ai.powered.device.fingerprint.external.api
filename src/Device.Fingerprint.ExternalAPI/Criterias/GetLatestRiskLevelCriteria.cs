using Fanex.Data;
using Fanex.Data.Repository;

namespace Device.Fingerprint.ExternalAPI.Criterias
{
    public class GetLatestRiskLevelCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_SubscriberID", AllowNull = true)]
        public int SubscriberId { get; set; }

        [SpParam(SqlParamName = "ip_LastID", AllowNull = true)]
        public long LastId { get; set; }

        [SpParam(SqlParamName = "ip_BatchSize", AllowNull = true)]
        public int BatchSize { get; set; }

        public override string GetSettingKey()
        {
            return "CTS_DC_CustClassification_GetLatestRiskLevel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }

}
