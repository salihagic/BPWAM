namespace BPWA.DAL.Models
{
    public class BusinessUnitSearchModel : BaseSearchModel
    {
        public string Name { get; set; }
        public int? CompanyId { get; set; }
    }
}
