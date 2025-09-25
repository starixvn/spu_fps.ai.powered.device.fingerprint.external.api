using System.Net;

namespace Device.Fingerprint.ExternalAPI.Models
{
    public class CustomerClassRiskLevelList
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public IEnumerable<CustomerClassRiskLevelModel>? Trans { get; set; }
    }

    public class CustomerClassRiskLevelModel
    {
        public long Id { get; set; }

        public string Account { get; set; } = string.Empty;

        public string RiskLevel { get; set; } = string.Empty;
    }
}
