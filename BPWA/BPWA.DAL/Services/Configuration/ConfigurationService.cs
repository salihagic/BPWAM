using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class ConfigurationService : BaseCRUDService<Configuration, ConfigurationSearchModel, ConfigurationDTO>, IConfigurationService
    {
        public ConfigurationService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public async Task<Configuration> FirstOrDefault()
        {
            return await DatabaseContext.Configuration
                .FirstOrDefaultAsync();
        }
    }
}
