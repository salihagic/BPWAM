using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class Pagination
    {
        public int? Skip { get; set; } = 0;
        public int? Take { get; set; } = 10;
        public int? Page { get; set; } = 0;
        public bool HasMore => Page < TotalNumberOfPages;
        public bool? ShouldTakeAllRecords { get; set; }
        public List<OrderField> OrderFields { get; set; } = new List<OrderField>();

        public int TotalNumberOfRecords { get; set; }
        public int TotalNumberOfPages => Take.GetValueOrDefault() == 0 ? 0 : (TotalNumberOfRecords % Take.GetValueOrDefault() != 0) ? (TotalNumberOfRecords / Take.GetValueOrDefault()) + 1 : TotalNumberOfRecords / Take.GetValueOrDefault();
    }
}
