using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Enumerations;
using BPWA.Common.Resources;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class AccountsService : IAccountsService
    {
        protected readonly DatabaseContext DatabaseContext;
        protected readonly IMapper Mapper;
        protected readonly UserManager<User> UserManager;
        protected readonly SignInManager<User> SignInManager;
        protected readonly ICurrentUser CurrentUser;
        protected readonly IEmailService EmailService;
        protected readonly RouteSettings RouteSettings;
        protected readonly IUsersService UsersService;

        public AppSettings AppSettings { get; }
        public ICompanyActivityStatusLogsService CompanyActivityStatusLogsService { get; }

        public AccountsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICurrentUser currentUser,
            IEmailService emailService,
            RouteSettings routeSettings,
            IUsersService usersService,
            AppSettings appSettings,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService
            )
        {
            DatabaseContext = databaseContext;
            Mapper = mapper;
            UserManager = userManager;
            SignInManager = signInManager;
            CurrentUser = currentUser;
            EmailService = emailService;
            RouteSettings = routeSettings;
            UsersService = usersService;
            AppSettings = appSettings;
            CompanyActivityStatusLogsService = companyActivityStatusLogsService;
        }

        public async Task<User> GetUserByUserNameOrEmail(string userName)
        {
            try
            {
                var user = await DatabaseContext.Users
                    .IgnoreQueryFilters()
                    .Where(x => x.UserName == userName || x.Email == userName)
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception(Translations.User_name_or_email_invalid);

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(Translations.User_name_or_email_invalid);
            }
        }

        public async Task UpdateTimezoneForCurrentUser(int timezoneUtcOffsetInMinutes)
        {
            var userResult = await UsersService.GetEntityById(CurrentUser.Id());

            var timezoneInfo = TimeZoneInfo.GetSystemTimeZones()
                                               .Where(x => x.BaseUtcOffset == (new TimeSpan(0, timezoneUtcOffsetInMinutes, 0)))
                                               .FirstOrDefault();

            if (timezoneInfo != null)
            {
                userResult.TimezoneId = timezoneInfo.Id;
                var result = await UsersService.Update(userResult);
            }
        }

        public async Task SendPasswordResetToken(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("Failed to load user");

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetUrl = GeneratePasswordResetUrl(user, token);

            await EmailService.Send(user.Email,
                                      "Change your password",
                                      $"You requested a reset of your password.\n\n\nClick on the following link to set your new password: {passwordResetUrl}");

            return;
        }

        protected string GeneratePasswordResetUrl(User user, string token)
        {
            return $"{RouteSettings.WebUrl}{RouteSettings.PasswordResetUrl}?userId={WebUtility.UrlEncode(user.Id)}&token={WebUtility.UrlEncode(token)}";
        }

        public async Task DeleteExpiredGuestAccounts()
        {
            var companies = await DatabaseContext.Companies
                .IgnoreQueryFilters()
                .Include(x => x.Subcompanies)
                .Include(x => x.CompanyActivityStatusLogs)
                .Include(x => x.Users)
                .Where(x => !x.IsDeleted)
                .Where(x => x.AccountType == AccountType.Guest)
                .Where(x => (x.CreatedAtUtc + AppSettings.GuestAccountLifespan) < DateTime.UtcNow)
                .ToListAsync();

            if (companies.Any())
            {
                DatabaseContext.RemoveRange(companies);
                await DatabaseContext.IgnoreSoftDeletableStamps().SaveChangesAsync();

                foreach (var company in companies)
                    await CompanyActivityStatusLogsService.NotifyClientsForCacheUpdate(company.Id);
            }
        }

        public async Task SendAccountDeactivationWarningNotifications()
        {
            if (!AppSettings.AccountLifespan.HasValue)
                return;

            var activeCompaniesSoonToExpire = await DatabaseContext.Companies
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted)
                .Where(x => x.AccountType == AccountType.Regular)
                .Where(x => (x.CompanyActivityStatusLogs
                    .Where(x => x.ActivityStatus == ActivityStatus.Active)
                    .OrderBy(x => x.ActivityEndUtc)
                    .Last().ActivityEndUtc - AppSettings.AccountDeactivationNotificationMargin) < DateTime.UtcNow
                    )
                .ToListAsync();

            if (activeCompaniesSoonToExpire.Any())
            {
                //Send notification and email
            }
        }
    }
}
