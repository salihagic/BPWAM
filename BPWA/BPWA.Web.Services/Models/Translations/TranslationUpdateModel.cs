namespace BPWA.Web.Services.Models
{
    public class TranslationUpdateModel : BaseUpdateModel
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
