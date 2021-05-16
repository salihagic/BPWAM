using BPWA.DAL.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Helpers.Middleware
{
    public class ActivityStatusMiddleware
    {
        private readonly RequestDelegate _next;
        private ICompanyActivityStatusLogsService _companyActivityStatusLogsService;
        private ICurrentBaseCompany _currentBaseCompany;
        private ICompaniesService _companiesService;
        private static List<ActivityStatusAllowedRoute> _allowedRoutes = new List<ActivityStatusAllowedRoute>
        {
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Deactivated" },
            new ActivityStatusAllowedRoute { Controller = "Authentication", Action = "Login" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Edit" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "ChangePassword" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "RegisterGuestAccountAndSignIn" },
            new ActivityStatusAllowedRoute { Controller = "Home", Action = "ChangeLanguage" },
        };
        private static ActivityStatusAllowedRoute DefaultRoute =
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Deactivated" };
        private static ActivityStatusAllowedRoute LoginRoute =
            new ActivityStatusAllowedRoute { Controller = "Authentication", Action = "Login" };

        public ActivityStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService,
            ICurrentBaseCompany currentBaseCompany,
            ICompaniesService companiesService
            )
        {
            _companyActivityStatusLogsService = companyActivityStatusLogsService;
            _currentBaseCompany = currentBaseCompany;
            _companiesService = companiesService;

            var doesNotHaveBaseCompanyId = !_currentBaseCompany.Id().HasValue;
            var isBaseCompanyActive = await _companyActivityStatusLogsService.IsActive(_currentBaseCompany.Id().GetValueOrDefault());

            var controller = context.Request.RouteValues["Controller"]?.ToString();
            var action = context.Request.RouteValues["Action"]?.ToString();
            var isAllowedRoute = _allowedRoutes.Any(x => x.Controller == controller && x.Action == action);

            if (doesNotHaveBaseCompanyId || isBaseCompanyActive || isAllowedRoute)
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
    }
}
