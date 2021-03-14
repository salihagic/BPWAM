﻿using BPWA.Core.Entities;
using BPWA.DAL.Models;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface IRolesService : IBaseService<Role, RoleSearchModel, RoleDTO, string>
    {
        Task<Role> GetEntityWithClaimsByName(string name);
    }
}
