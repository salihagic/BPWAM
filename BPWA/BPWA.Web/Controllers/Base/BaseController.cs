using BootstrapBreadcrumbs.Core;
using BPWA.Common.Extensions;
using BPWA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            ShouldResetNavigationStack = true;
        }

        public virtual IActionResult Error() => RedirectToAction("Error", "Home");
        public virtual IActionResult _Error() => RedirectToAction("_Error", "Home");

        #region Navigation

        protected bool ShouldResetNavigationStack { get; set; }

        protected string __sessionNavigationStackKey__ = $"__navigation__stack__";
        private List<BreadcrumbsItem> _sessionNavigationStack
        {
            get => HttpContext.Session.GetObject<List<BreadcrumbsItem>>(__sessionNavigationStackKey__) ?? new List<BreadcrumbsItem>();
            set => HttpContext.Session.SetObject(__sessionNavigationStackKey__, value);
        }

        protected string _defaultAction;
        protected string DefaultAction
        {
            get => _defaultAction = "Index";
            set => _defaultAction = value;
        }

        protected string _currentAction;
        protected string CurrentAction
        {
            get => _currentAction = HttpContext.Request.RouteValues["Action"].ToString();
            set => _currentAction = value;
        }

        protected string _currentController;
        protected string CurrentController
        {
            get => _currentController = HttpContext.Request.RouteValues["Controller"].ToString();
            set => _currentController = value;
        }

        public string CurrentBreadcrumbItem { get; set; }

        protected void ResetNavigationStack()
        {
            var stack = _sessionNavigationStack;
            stack.Clear();
            _sessionNavigationStack = stack;
        }

        protected void BreadcrumbItem(string title = null, object routeValues = null)
        {
            var currentNavigationItem = new BreadcrumbsItem
            {
                Title = string.IsNullOrEmpty(title) ? TranslationsHelper.Translate(CurrentAction) : title,
                Controller = CurrentController,
                Action = CurrentAction,
                RouteValues = routeValues
            };

            if (_sessionNavigationStack.Count > 0)
            {
                var last = _sessionNavigationStack.Last();

                if (last.Controller == currentNavigationItem.Controller && last.Action == currentNavigationItem.Action)
                {
                    var stack1 = _sessionNavigationStack;
                    stack1.RemoveAt(stack1.Count - 1);
                    _sessionNavigationStack = stack1;
                }
                else
                {
                    this.SetBreadcrumbPrefixItems(_sessionNavigationStack);
                }
            }

            this.SetBreadcrumbAction(currentNavigationItem);

            ViewBag.Title = currentNavigationItem.Title;

            var stack = _sessionNavigationStack;
            stack.Add(currentNavigationItem);
            _sessionNavigationStack = stack;
        }

        protected void BreadcrumbPrefixItem(string title = null, string action = null, string controller = null, object routeValues = null)
        {
            this.SetBreadcrumbPrefixItems(new List<BreadcrumbsItem>
            {
                new BreadcrumbsItem
                {
                    Title = string.IsNullOrEmpty(title) ? TranslationsHelper.Translate(CurrentController) : title,
                    Controller = string.IsNullOrEmpty(controller) ? CurrentController : controller,
                    Action = string.IsNullOrEmpty(action) ? DefaultAction : action,
                    RouteValues = routeValues
                }
            });
        }

        #endregion Navigation
    }
}
