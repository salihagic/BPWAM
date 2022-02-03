using AutoMapper;
using BPWA.DAL.Database;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class ConfigurationWebService : ConfigurationService, IConfigurationWebService
    {
        public ConfigurationWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }

        public async Task<ConfigurationUpdateModel> PrepareForUpdate()
        {
            var entity = await FirstOrDefault();
            return Mapper.Map<ConfigurationUpdateModel>(entity);
        }

        public async Task Update(ConfigurationUpdateModel model)
        {
            try
            {
                var entity = await FirstOrDefault();

                Mapper.Map(model, entity);

                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Failed to update configuration");
            }
        }
    }
}
