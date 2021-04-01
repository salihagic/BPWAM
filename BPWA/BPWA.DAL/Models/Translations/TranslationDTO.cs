namespace BPWA.DAL.Models
{
    public class TranslationDTO : BaseDTO
    {
        public string Culture { get; set; }
        public string Key { get; set; }
        public string KeyHash { get; set; }
        public string Value { get; set; }
    }
}
