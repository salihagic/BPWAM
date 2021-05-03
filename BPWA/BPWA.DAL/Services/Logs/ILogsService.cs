using BPWA.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public interface ILogsService
    {
        Task<List<LogDTO>>  Get(LogSearchModel searchModel = null);
        Task<LogDTO> GetById(int id);
    }
}
