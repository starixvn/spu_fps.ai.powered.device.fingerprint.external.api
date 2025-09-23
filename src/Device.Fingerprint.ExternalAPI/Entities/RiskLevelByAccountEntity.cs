namespace Device.Fingerprint.ExternalAPI.Entities
{
    public class RiskLevelByAccountEntity
    {
        public long AccountID { get; set; }

        public string RiskLevel { get; set; } = string.Empty;

    }
}
