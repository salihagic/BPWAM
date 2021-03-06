using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Currency : BaseSoftDeletableEntity
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public List<CountryCurrency> CountryCurrencies { get; set; }
    }
}
