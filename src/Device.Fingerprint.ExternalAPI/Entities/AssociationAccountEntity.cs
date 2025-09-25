namespace Device.Fingerprint.ExternalAPI.Entities
{
    public class AssociationAccountEntity
    {
        public long AccountID { get; set; }

        public string Account { get; set; } = string.Empty;

        public DateTime FirstAssociationDate { get; set; }
    }
}
