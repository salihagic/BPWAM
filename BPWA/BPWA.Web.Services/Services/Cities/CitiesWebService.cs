using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class CitiesWebService : CitiesService, ICitiesWebService
    {
        public CitiesWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }

        public override IQueryable<City> BuildIncludesById(int id, IQueryable<City> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.Country);
        }

        public override IQueryable<City> BuildIncludes(IQueryable<City> query)
        {
            return base.BuildIncludes(query)
                       .Include(x => x.Country);
        }
    }
}
