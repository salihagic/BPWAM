namespace BPWA.DAL.Models
{
    public class TranslationSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
