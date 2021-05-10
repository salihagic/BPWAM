namespace BPWA.DAL.Models
{
    public class CityDTO : 
        BaseSoftDeletableDTO,
        IBaseSoftDeletableDTO,
        IBaseDTO
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }
    }
}
