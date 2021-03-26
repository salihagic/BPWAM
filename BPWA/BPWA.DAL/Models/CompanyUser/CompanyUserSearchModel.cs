namespace BPWA.DAL.Models
{
    public class CompanyUserSearchModel : BaseSearchModel
    {
        public int? CompanyId { get; set; }
        public string UserId { get; set; }
    }
}
