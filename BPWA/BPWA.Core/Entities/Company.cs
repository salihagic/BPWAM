using BPWA.Common.Enumerations;
using System.Collections.Generic;

namespace BPWA.Core.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public AccountType AccountType { get; set; }
        /// <summary>
        /// Refers ActivityStatus in CompanyActivityStatusLogs last created record
        /// </summary>
        public ActivityStatus ActivityStatus { get; set; }

        public List<User> Users { get; set; }
        public List<Role> Roles { get; set; }
        public List<Company> Subcompanies { get; set; }
        public List<CompanyActivityStatusLog> CompanyActivityStatusLogs { get; set; }
    }
}
