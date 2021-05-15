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
        private List<ActivityStatusAllowedRoute> _allowedRoutes = new List<ActivityStatusAllowedRoute>
        {
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Deactivated" },
            new ActivityStatusAllowedRoute { Controller = "Authentication", Action = "Login" },
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Edit" },
        };
        private ActivityStatusAllowedRoute DefaultRoute =
            new ActivityStatusAllowedRoute { Controller = "Account", Action = "Deactivated" };

        public ActivityStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService,
            ICurrentBaseCompany currentBaseCompany
            )
        {
            _companyActivityStatusLogsService = companyActivityStatusLogsService;
            _currentBaseCompany = currentBaseCompany;

            if (!_currentBaseCompany.Id().HasValue ||
                await _companyActivityStatusLogsService.IsActive(_currentBaseCompany.Id().GetValueOrDefault()) ||
                _allowedRoutes.Any(x =>
                    x.Controller == context.Request.RouteValues["Controller"]?.ToString() &&
                    x.Action == context.Request.RouteValues["Action"]?.ToString()
                ))
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.Redirect(DefaultRoute.Location);
            }
        }
    }
}
