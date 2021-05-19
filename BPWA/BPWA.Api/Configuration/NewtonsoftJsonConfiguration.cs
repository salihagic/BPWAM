using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BPWA.Api.Configuration
{
    public static class NewtonsoftJsonConfiguration
    {
        public static IMvcBuilder ConfigureNewtonsoftJson(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            return mvcBuilder;
        }
    }
}
