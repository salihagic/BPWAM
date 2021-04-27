using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class UserDTO : BaseDTO<string>, IBaseDTO<string>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TimezoneId { get; set; }
        public int? CityId { get; set; }

        public CityDTO City { get; set; }
        public List<UserRoleDTO> UserRoles { get; set; }
        public List<CompanyUserDTO> CompanyUsers { get; set; }
        public List<BusinessUnitUserDTO> BusinessUnitUsers { get; set; }
    }
}
