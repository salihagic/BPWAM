namespace BPWA.DAL.Models
{
    public class CitySearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
    }
}
