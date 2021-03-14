using BPWA.DAL.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace BPWA.Web.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperReferenceClassDAL).Assembly,
                                   typeof(AutoMapperReferenceClassWeb).Assembly);

            return services;
        }
    }
}
