namespace Device.Fingerprint.ExternalAPI.Entities
{
    public class AssociationAccountUsageEntity
    {
        public string LoginName { get; set; } = string.Empty;

        public DateTime MnUsageDate { get; set; }

        public DateTime MaxUsageDate { get; set; }

        public int UsageCount { get; set; }
    }
}
