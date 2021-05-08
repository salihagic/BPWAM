using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Resources;
using BPWA.Common.Security;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Web.Services.Services
{
    public class DropdownHelperService : IDropdownHelperService
    {
        private ICurrentUser _currentUser;

        public DropdownHelperService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        virtual public List<DropdownItem<string>> GetAppClaims()
        {
            var claims = new List<string>();

            if (_currentUser.HasGodMode() || 
                (_currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.RolesManagement) && !_currentUser.CurrentCompanyId().HasValue))
            {
                claims.AddRange(AppClaimsHelper.Authorization.Administration.All);
                claims.AddRange(AppClaimsHelper.Authorization.Company.All);
            }
            else if (_currentUser.HasCompanyGodMode() || _currentUser.HasAuthorizationClaim(AppClaims.Authorization.Company.RolesManagement))
            {
                claims.AddRange(AppClaimsHelper.Authorization.Company.All);
            }

            return claims.Select(x => new DropdownItem<string>
            {
                Id = x,
                Text = TranslationsHelper.Translate(x)
            }).ToList();
        }

        public List<DropdownItem<string>> GetTicketStatuses()
        {
            return Enum.GetValues(typeof(TicketStatuses)).Cast<TicketStatuses>()
                .Select(x => new DropdownItem<string>
                {
                    Id = x.ToString(),
                    Text = TranslationsHelper.Translate(x.ToString())
                }).ToList();
        }

        public List<DropdownItem<string>> GetTicketTypes()
        {
            return Enum.GetValues(typeof(TicketTypes)).Cast<TicketTypes>()
                .Select(x => new DropdownItem<string>
                {
                    Id = x.ToString(),
                    Text = TranslationsHelper.Translate(x.ToString())
                }).ToList();
        }

        public List<DropdownItem<string>> GetSystemLanguages()
        {
            return TranslationOptions.SupportedLanguages
                .Select(x => new DropdownItem<string>
                {
                    Id = x.CultureInfo.Name,
                    Text = x.Name
                }).ToList();
        }

        public List<DropdownItem<string>> GetNotificationTypes()
        {
            return Enum.GetValues(typeof(NotificationType)).Cast<NotificationType>()
                .Select(x => new DropdownItem<string>
                {
                    Id = x.ToString(),
                    Text = TranslationsHelper.Translate(x.ToString())
                }).ToList();
        }

        public List<DropdownItem<string>> GetNotificationDistributionTypes()
        {
            return Enum.GetValues(typeof(NotificationDistributionType)).Cast<NotificationDistributionType>()
                .Select(x => new DropdownItem<string>
                {
                    Id = x.ToString(),
                    Text = TranslationsHelper.Translate(x.ToString())
                }).ToList();
        }


        public List<DropdownItem<string>> GetNotificationDistributionTypes1()
        {
            return GetDropDown(
                Enum.GetValues(typeof(NotificationDistributionType)).Cast<NotificationDistributionType>().ToList(),
                item => new DropdownItem<string> { Id = item.ToString(), Text = TranslationsHelper.Translate(item.ToString()) },
                Translations.Select_notification_distribution_type);
        }

        public List<DropdownItem<string>> GetDropDown<T>(List<T> list, Func<T, DropdownItem<string>> getObject, string defaultText = "")
        {
            var selectList = new List<DropdownItem<string>>();

            if (defaultText.IsNotEmpty())
                selectList.Add(new DropdownItem<string>() { Id = string.Empty, Text = defaultText });

            foreach (var item in list)
                selectList.Add(getObject(item));

            return selectList;
        }
    }
}
