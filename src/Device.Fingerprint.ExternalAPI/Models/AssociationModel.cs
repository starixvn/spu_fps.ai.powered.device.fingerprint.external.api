using System.Net;

namespace Device.Fingerprint.ExternalAPI.Models
{
    public class AssociationList
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public int AssociatedDevices { get; set; }

        public IEnumerable<AssociationDeviceModel>? AssociatedDeviceDetails { get; set; }

        public int AssociatedAccounts { get; set; }

        public IEnumerable<AssociationAccountModel>? AssociatedAccountDetails { get; set; }
    }

    public class AssociationDeviceModel
    {
        public string Device { get; set; } = string.Empty;

        public DateTime FirstSeenDate { get; set; }
    }

    public class AssociationAccountModel
    {
        public string Account { get; set; } = string.Empty;

        public DateTime FirstAssociationDate { get; set; }
    }
}
