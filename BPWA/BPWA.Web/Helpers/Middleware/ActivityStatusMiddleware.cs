using BPWA.DAL.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Helpers.Middleware
{
    public class ActivityStatusMiddleware
    {
        public async Task InvokeAsync(
            HttpContext context,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService,
            ICurrentBaseCompany currentBaseCompany,
            ICompaniesService companiesService
            )
        {
            _context = context;
            _companyActivityStatusLogsService = companyActivityStatusLogsService;
            _currentBaseCompany = currentBaseCompany;
            _companiesService = companiesService;

            if (await IsAuthorized())
            {
                await _next.Invoke(context);
            }
            else
            {
                var exists = await _companiesService.Exists(_currentBaseCompany.Id().GetValueOrDefault());

                if (exists)
                    context.Response.Redirect(DefaultRoute.Location);
                else
                    context.Response.Redirect(LoginRoute.Location);
            }
        }

        private HttpContext _context;
        private RequestDelegate _next;
        private ICompanyActivityStatusLogsService _companyActivityStatusLogsService;
        private ICurrentBaseCompany _currentBaseCompany;
        private ICompaniesService _companiesService;
        private static List<ActivityStatusAllowedRoute> _allowedRoutes = new List<ActivityStatusAllowedRoute>
        {
            new ActivityStatusAllowedRoute { Controller = "Activations", Action = "Deactivated" },
            new ActivityStatusAllowedRoute { Controller = "Authentication", Action = "Login" },
            new ActivityStatusAllowedRoute { Controller = "Authentication", Action = "Logout" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Edit" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "ChangePassword" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "RegisterGuestAccountAndSignIn" },
            new ActivityStatusAllowedRoute { Controller = "Home", Action = "ChangeLanguage" },
        };
        private static ActivityStatusAllowedRoute DefaultRoute =
            new ActivityStatusAllowedRoute { Controller = "Activations", Action = "Deactivated" };
        private static ActivityStatusAllowedRoute LoginRoute =
            new ActivityStatusAllowedRoute { Controller = "Authentication", Action = "Login" };

        public ActivityStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        async Task<bool> IsAuthorized()
        {
            var doesNotHaveBaseCompanyId = !_currentBaseCompany.Id().HasValue;
            var isBaseCompanyActive = await _companyActivityStatusLogsService.IsActive(_currentBaseCompany.Id().GetValueOrDefault());

            var controller = _context.Request.RouteValues["Controller"]?.ToString();
            var action = _context.Request.RouteValues["Action"]?.ToString();

            var isAllowedRoute = _allowedRoutes.Any(x => x.Controller == controller && x.Action == action);

            return doesNotHaveBaseCompanyId || isBaseCompanyActive || isAllowedRoute;
        }
    }
}
