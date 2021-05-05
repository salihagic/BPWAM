namespace BPWA.Core.Entities
{
    public interface IBaseCompanyRelatedEntity : IBaseCompanyRelatedEntity<int> { }

    public interface IBaseCompanyRelatedEntity<TKey>
    {
        public TKey Id { get; set; }
        public int? CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
