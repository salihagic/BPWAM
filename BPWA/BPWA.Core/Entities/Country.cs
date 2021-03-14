using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Country : BaseEntity, IBaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public List<City> Cities { get; set; }
        public List<CountryCurrency> CountryCurrencies { get; set; }
        public List<CountryLanguage> CountryLanguages { get; set; }
    }
}
