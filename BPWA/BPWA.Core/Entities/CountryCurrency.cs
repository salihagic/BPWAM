namespace BPWA.Core.Entities
{
    public class CountryCurrency : BaseSoftDeletableEntity
    {
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }

        public Country Country { get; set; }
        public Currency Currency { get; set; }
    }
}
