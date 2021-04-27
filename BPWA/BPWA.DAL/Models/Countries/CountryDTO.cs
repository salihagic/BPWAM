using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class CountryDTO : BaseDTO, IBaseDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public List<CurrencyDTO> Currencies { get; set; }
        public List<LanguageDTO> Languages { get; set; }
    }
}
