namespace BPWA.Core.Entities
{
    public interface IBaseCompanyEntity
    {
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
