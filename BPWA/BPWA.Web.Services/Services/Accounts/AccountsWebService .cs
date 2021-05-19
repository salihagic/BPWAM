using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Enumerations;
using BPWA.Common.Exceptions;
using BPWA.Common.Resources;
using BPWA.Common.Security;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class AccountsWebService :
        AccountsService,
        IAccountsWebService
    {
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly ICurrentBaseCompany _currentBaseCompany;

        public AccountsWebService(
            IMapper mapper,
            AppSettings appSettings,
            ICurrentUser currentUser,
            IEmailService emailService,
            IUsersService usersService,
            RouteSettings routeSettings,
            UserManager<User> userManager,
            DatabaseContext databaseContext,
            SignInManager<User> signInManager,
            ICompaniesService companiesService,
            ICurrentBaseCompany currentBaseCompany,
            INotificationsService notificationsService,
            BackgroundServicesSettings backgroundServicesSettings,
            ICompanyActivityStatusLogsService companyActivityStatusLogsService
            ) : base(
                appSettings,
                currentUser,
                emailService,
                usersService,
                routeSettings,
                userManager,
                databaseContext,
                companiesService,
                notificationsService,
                backgroundServicesSettings,
                companyActivityStatusLogsService
                )
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _currentBaseCompany = currentBaseCompany;
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            var user = await GetUserByIdWithoutQueryFilters(CurrentUser.Id());

            var result = await UserManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(x => x.Description).ToArray());
        }

        public async Task<UserDTO> RegisterGuestAccountAndSignIn()
        {
            #region Add company

            var company = new Company
            {
                Name = $"Guest Company",
                AccountType = AccountType.Guest,
                Roles = new List<Role>
                {
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Company admin",
                        NormalizedName = "Company admin".ToUpper(),
                        RoleClaims = new List<RoleClaim>
                        {
                            new RoleClaim
                            {
                                ClaimType = AppClaimsHelper.Authorization.Type,
                                ClaimValue = AppClaims.Authorization.Company.CompanyGodMode
                            }
                        }
                    }
                }
            };

            await DatabaseContext.Companies.AddAsync(company);
            await DatabaseContext.IgnoreCompanyStamps().SaveChangesAsync();

            #endregion

            #region Add activity status

            await CompanyActivityStatusLogsService.Add(new CompanyActivityStatusLog
            {
                CompanyId = company.Id,
                ActivityStatus = ActivityStatus.Active,
                ActivityStartUtc = DateTime.UtcNow,
                ActivityEndUtc = DateTime.UtcNow + AppSettings.AccountLifespan
            });

            #endregion

            #region Add admin user

            var companyAdmin = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = $"guest.admin.{company.Id}",
                NormalizedUserName = $"guest.admin.{company.Id}".ToUpper(),
                Email = $"guest.admin.{company.Id}@BPWAM.com",
                NormalizedEmail = $"guest.admin.{company.Id}@BPWAM.com".ToUpper(),
                FirstName = "Guest",
                LastName = "Admin",
                CompanyId = company.Id,
                CurrentCompanyId = company.Id,
                UserRoles = company.Roles.Select(x => new UserRole
                {
                    RoleId = x.Id,
                    CompanyId = x.CompanyId
                }).ToList(),
            };

            await DatabaseContext.Users.AddAsync(companyAdmin);
            await DatabaseContext.IgnoreCompanyStamps().SaveChangesAsync();

            var result = await UserManager.AddPasswordAsync(companyAdmin, "demo");

            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(x => x.Description).ToArray());

            await _signInManager.SignInAsync(companyAdmin, true);

            #endregion

            #region Add notification

            var expirationDateTime = DateTime.UtcNow.Add(AppSettings.GuestAccountLifespan);

            var expirationDateTimeString = $"{expirationDateTime.ToString("dd.MM.yyyy HH:mm:ss")} UTC"; 

            await NotificationsService.Add(new Notification
            {
                NotificationDistributionType = NotificationDistributionType.SingleUser,
                CompanyId = company.Id,
                UserId = companyAdmin.Id,
                NotificationType = NotificationType.GuestAccountExpiration,
                Title = "Account expiration",
                Description = $"Your company {company.Name} and all related data will be deleted at {expirationDateTimeString} if you do not convert the company account type to a regular one.",
            });

            #endregion

            return _mapper.Map<UserDTO>(companyAdmin);
        }

        public async Task ConvertFromGuestToRegular()
        {
            var companies = await DatabaseContext.Companies
                .IgnoreQueryFilters()
                .Where(x => !x.IsDeleted)
                .Where(x =>
                //All
                ((_currentBaseCompany.Id() == null && x.AccountType == AccountType.Regular) ||
                //Level 0 company
                (x.Id == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 1 company
                (x.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 2 company
                (x.Company.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 3 company
                (x.Company.Company.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular)) ||
                //Level 4 company
                (x.Company.Company.Company.CompanyId == _currentBaseCompany.Id() && (_currentBaseCompany.IsGuest() || x.AccountType == AccountType.Regular))
                //...
                )).ToListAsync();

            companies.ForEach(x => x.AccountType = AccountType.Regular);

            DatabaseContext.Companies.UpdateRange(companies);
            await DatabaseContext.SaveChangesAsync();

            await DeleteGuestAccountExpirationNotifications(_currentBaseCompany.Id().GetValueOrDefault());

            await RefreshSignIn();
        }

        async Task DeleteGuestAccountExpirationNotifications(int companyId)
        {
            var notifications = await DatabaseContext.Notifications
                .Include(x => x.NotificationLogs)
                .Where(x => x.NotificationType == NotificationType.GuestAccountExpiration)
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();

            DatabaseContext.RemoveRange(notifications);

            await DatabaseContext.SaveChangesAsync();
        }

        #region Update account 

        public async Task<AccountUpdateModel> PrepareForUpdate()
        {
            try
            {
                var user = await DatabaseContext.Users
                    .AsNoTracking()
                    .IgnoreQueryFilters()
                    .Include(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == CurrentUser.Id());

                var model = _mapper.Map<AccountUpdateModel>(user);

                return model;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load the user");
            }
        }

        public async Task Update(AccountUpdateModel model)
        {
            try
            {
                var user = await DatabaseContext.Users
                    .IgnoreQueryFilters()
                    .Include(x => x.City)
                    .FirstOrDefaultAsync(x => x.Id == CurrentUser.Id());

                _mapper.Map(model, user);

                await DatabaseContext.SaveChangesAsync();

                await RefreshSignIn();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to update account");
            }
        }

        #endregion

        public async Task<User> GetUserByIdWithoutQueryFilters(string id)
        {
            return await DatabaseContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task ToggleCurrentCompany(ToggleCurrentCompanyModel model)
        {
            //TODO: Check if the user can change to selected company
            //if (!CurrentUser.HasGodMode() && !(CurrentUser.CompanyIds().Count > 1))
            //    return Result.Failed(Translations.There_was_an_error_while_trying_to_change_current_company);

            var currentUser = await GetUserByIdWithoutQueryFilters(CurrentUser.Id());

            if (model.CompanyId == 0)
                model.CompanyId = currentUser.CompanyId;

            try
            {
                currentUser.CurrentCompanyId = model.CompanyId;

                DatabaseContext.Users.Update(currentUser);
                await DatabaseContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(Translations.There_was_an_error_while_trying_to_change_current_company);
            }

            await RefreshSignIn();
        }

        public async Task RefreshSignIn()
        {
            var currentUser = await GetUserByIdWithoutQueryFilters(CurrentUser.Id());

            await _signInManager.RefreshSignInAsync(currentUser);
        }

        public async Task<ResetPasswordModel> PrepareForResetPassword(string userId, string token)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("Failed to load user");

            var model = new ResetPasswordModel
            {
                UserId = userId,
                Token = token
            };

            return model;
        }

        public async Task ResetPassword(ResetPasswordModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);

            if (user == null)
                throw new Exception("Failed to load user");

            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
        }
        
        public async Task<UserDTO> SignIn(string userName, string password)
        {
            var userResult = await GetUserByUserNameOrEmail(userName);

            var result = await _signInManager.PasswordSignInAsync(userResult, password, true, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    throw new Exception(Translations.Account_locked_out);
                else if (result.IsNotAllowed)
                    throw new Exception(Translations.Login_not_allowed);
                else if (result.RequiresTwoFactor)
                    throw new Exception(Translations.Login_required_two_factor);
                else
                    throw new Exception(Translations.User_name_or_email_invalid);
            }

            var userDTO = _mapper.Map<UserDTO>(userResult);

            return userDTO;
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
