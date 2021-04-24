using BPWA.Common.Enumerations;
using System;

namespace BPWA.DAL.Models
{
    public class LogSearchModel : BaseSearchModel
    {
        public string SearchTerm { get; set; }
        public LogEventLevel Level { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
