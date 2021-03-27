using BPWA.DAL.Database;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace BPWA.Web.Helpers.Filters
{
    public class Transaction : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();

            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                var result = await next();

                if (result.Exception != null)
                    transaction.Rollback();
                else
                    transaction.Commit();
            }
        }
    }
}
