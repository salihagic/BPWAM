﻿namespace BPWA.DAL.Models
{
    public class CurrencySearchModel : BaseSearchModel
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
