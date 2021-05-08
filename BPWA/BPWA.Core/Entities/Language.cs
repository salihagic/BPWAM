using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Language : BaseSoftDeletableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public List<CountryLanguage> CountryLanguages { get; set; }
    }
}
