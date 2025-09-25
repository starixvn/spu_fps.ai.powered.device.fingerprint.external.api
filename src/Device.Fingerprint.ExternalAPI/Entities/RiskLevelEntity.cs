namespace Device.Fingerprint.ExternalAPI.Entities
{
    public class RiskLevelEntity
    {
        public long Id { get; set; }

        public string RegisterName { get; set; } = string.Empty;

        public string RiskLevel { get; set; } = string.Empty;

        public long CustID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CustomerClassName { get; set; } = string.Empty;

        public string CustomerClass { get; set; } = string.Empty;

        public DateTime LastModifiedDate { get; set; }

    }
}
