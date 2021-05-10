using BPWA.Common.Resources;

namespace BPWA.DAL.Models
{
    public class TranslationDTO : 
        BaseDTO, 
        IBaseDTO
    {
        public string Culture { get; set; }
        public string Language => TranslationOptions.GetByCulture(Culture)?.Name;
        public string Key { get; set; }
        public string KeyHash { get; set; }
        public string Value { get; set; }
    }
}
