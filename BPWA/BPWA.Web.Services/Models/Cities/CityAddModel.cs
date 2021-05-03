namespace BPWA.Web.Services.Models
{
    public class CityAddModel
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int? CountryId { get; set; }
        public DropdownItem CountryIdDropdownItem { get; set; }
    }
}
