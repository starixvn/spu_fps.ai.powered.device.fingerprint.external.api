using System.Net;

namespace Device.Fingerprint.ExternalAPI.Models
{
    public class AssociationAccountUsageList
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string FromDate { get; set; } = string.Empty;

        public string ToDate { get; set; } = string.Empty;

        public int AssociatedAccounts { get; set; }

        public IEnumerable<AssociationAccountUsageModel>? AssociatedAccountDetails { get; set; }
    }

    public class AssociationAccountUsageModel
    {
        public string Account { get; set; } = string.Empty;

        public string MinUsageDate { get; set; } = string.Empty;

        public string MaxUsageDate { get; set; } = string.Empty;

        public int UsageCount { get; set; }
    }

}
