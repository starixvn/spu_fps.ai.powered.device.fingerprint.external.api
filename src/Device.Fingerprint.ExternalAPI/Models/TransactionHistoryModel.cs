using System.Net;

namespace Device.Fingerprint.ExternalAPI.Models
{
    public class TransactionHistoryList
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public int TotalTrans
        {
            get
            {
                return Trans.Count();
            }
        }

        public IEnumerable<TransactionHistoryModel>? Trans { get; set; }
    }


    public class TransactionHistoryModel
    {
        public DateTime DateTime { get; set; }

        public long TrackingNumber { get; set; }

        public string Account { get; set; } = string.Empty;

        public string RiskLevel { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string Device { get; set; } = string.Empty;

        public string DeviceType { get; set; } = string.Empty;

        public string Browser { get; set; } = string.Empty;

        public string Domain { get; set; } = string.Empty;

        public string IP { get; set; } = string.Empty;

        public string RobotTracking { get; set; } = string.Empty;

    }
}
