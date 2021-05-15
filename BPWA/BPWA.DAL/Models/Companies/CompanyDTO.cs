using BPWA.Common.Enumerations;
using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class CompanyDTO :
        BaseDTO,
        IBaseDTO
    {
        public string Name { get; set; }
        /// <summary>
        /// Refers ActivityStatus in CompanyActivityStatusLogs last created record
        /// </summary>
        public ActivityStatus ActivityStatus { get; set; }

        public List<UserDTO> Users { get; set; }
        public List<CompanyDTO> Subcompanies { get; set; }
    }
}
