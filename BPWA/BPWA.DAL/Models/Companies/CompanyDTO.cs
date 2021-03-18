﻿using System.Collections.Generic;

namespace BPWA.DAL.Models
{
    public class CompanyDTO : BaseDTO
    {
        public string Name { get; set; }

        public List<UserDTO> Users { get; set; }
    }
}