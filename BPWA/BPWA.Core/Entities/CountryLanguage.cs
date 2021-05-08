namespace BPWA.Core.Entities
{
    public class CountryLanguage : BaseSoftDeletableEntity
    {
        public int CountryId { get; set; }
        public int LanguageId { get; set; }

        public Country Country { get; set; }
        public Language Language { get; set; }
    }
}
