namespace BPWA.DAL.Database
{
    public class DatabaseContextOptions
    {
        public void Reset()
        {
            IgnoreCompanyStampsOnSaveChanges = false;
            IgnoreAuditableStampsOnSaveChanges = false;
            IgnoreSoftDeletableStampsOnSaveChanges = false;
        }

        public bool IgnoreCompanyStampsOnSaveChanges { get; set; }
        public bool IgnoreAuditableStampsOnSaveChanges { get; set; }
        public bool IgnoreSoftDeletableStampsOnSaveChanges { get; set; }
    }
}
