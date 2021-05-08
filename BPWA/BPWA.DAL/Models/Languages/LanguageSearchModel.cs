namespace BPWA.DAL.Models
{
    public class LanguageSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
