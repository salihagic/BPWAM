namespace BPWA.Core.Entities
{
    public interface IBaseCompanyEntity :
        IBaseCompanyEntity<int> 
    { }

    public interface IBaseCompanyEntity<TKey> : IBaseEntity<TKey>
    {
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
