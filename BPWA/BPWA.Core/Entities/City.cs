using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class City : 
        BaseEntity,
        IBaseEntity
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public List<User> Users { get; set; }
    }
}
