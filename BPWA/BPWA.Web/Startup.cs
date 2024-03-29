using BPWA.Web.Configuration;
using BPWA.Web.Helpers.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BPWA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureIdentity()
                    .ConfigureAppSettings(Configuration)
                    .ConfigureAppServices()
                    .ConfigureDatabase()
                    .ConfigureAuthorizationPolicies()
                    .ConfigureAutoMapper()
                    .AddHttpContextAccessor()
                    .AddSession()
                    .AddHttpClient()
                    .ConfigureLocalization()
                    .AddControllersWithViews() //returns IMvcBuilder
                    .ConfigureNewtonsoftJson()
                    .ConfigureFluentValidation()
                    .AddViewLocalization()
                    .ConfigureDataAnnotationsLocalization()
                    .AddRazorRuntimeCompilation()
                    .ConfigureToastNotifications();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy(app.ApplicationServices.GetService<IOptions<CookiePolicyOptions>>().Value);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseNToastNotify();

            app.UseMiddleware<ActivityStatusMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                else
                    endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
