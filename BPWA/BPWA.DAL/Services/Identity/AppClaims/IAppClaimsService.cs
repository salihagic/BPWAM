using BPWA.DAL.Models;
using System.Collections.Generic;
using TFM.DAL.Models;

namespace BPWA.DAL.Services
{
    public interface IAppClaimsService
    {
        Result<List<string>> Get(AppClaimsSearchModel searchModel = null);
    }
}
