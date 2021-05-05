namespace BPWA.Core.Entities
{
    public class BaseCompanyEntity : BaseCompanyEntity<int> { }

    public class BaseCompanyEntity<TKey> :
        BaseEntity<TKey>,
        IBaseCompanyEntity<TKey>,
        IBaseEntity<TKey>,
        IBaseSoftDeletableEntity
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
