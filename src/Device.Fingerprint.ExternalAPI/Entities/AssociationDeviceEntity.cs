namespace Device.Fingerprint.ExternalAPI.Entities
{
    public class AssociationDeviceEntity
    {
        public long DeviceID { get; set; }

        public string Device { get; set; } = string.Empty;

        public DateTime FirstSeenDate { get; set; }
    }
}
