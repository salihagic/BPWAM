using AutoMapper;
using BPWA.Common.Resources;
using BPWA.Common.Security;
using BPWA.Controllers;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.Web.Helpers.Filters;
using BPWA.Web.Services.Models;
using BPWA.Web.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace BPWA.Administration.Controllers
{
    [Authorize(Policy = AppClaims.Authorization.Administration.ConfigurationManagement)]
    public class ConfigurationController : BaseController
    {
        private IConfigurationWebService _configurationService;
        private IToastNotification _toast;

        public ConfigurationController(
            IConfigurationWebService service,
            IToastNotification toast
            )
        {
            _configurationService = service;
            _toast = toast;
        }

        public async Task<IActionResult> Edit()
        {
            try
            {
                ResetNavigationStack();
                BreadcrumbItem(Translations.Configuration);

                var result = await _configurationService.PrepareForUpdate();

                return View(result);
            }
            catch (Exception)
            {
                return Error();
            }
        }

        [HttpPost, Transaction]
        public virtual async Task<IActionResult> Edit(ConfigurationUpdateModel model)
        {
            ViewBag.Title = Translations.Configuration;

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _configurationService.Update(model);

                _toast.AddSuccessToastMessage("Successfully edited configuration");
                return RedirectToAction(nameof(Edit));
            }
            catch (Exception)
            {
                _toast.AddErrorToastMessage("Failed to edit configuration");
            }

            return View(model);
        }
    }
}
