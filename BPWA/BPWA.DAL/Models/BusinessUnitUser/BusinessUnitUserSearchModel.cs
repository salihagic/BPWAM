namespace BPWA.DAL.Models
{
    public class BusinessUnitUserSearchModel : BaseSearchModel
    {
        public int? BusinessUnitId { get; set; }
        public string UserId { get; set; }
    }
}
