namespace Device.Fingerprint.ExternalAPI.Entities
{
    public class TransactionHistoryEntity
    {
        public long TransID { get; set; }

        public DateTime TransTime { get; set; }

        public int SubscriberID { get; set; }

        public string UserName { get; set; } = string.Empty;

        public long AccountID { get; set; }

        public string FirstDeviceCode { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string ActionResult { get; set; } = string.Empty;

        public string OS { get; set; } = string.Empty;

        public string Browser { get; set; } = string.Empty;

        public string UrlDetails { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        public string RobotTracking { get; set; } = string.Empty;

        public long RawFingerPrintID { get; set; }

        public long RawUserAgentID { get; set; }
    }
}
