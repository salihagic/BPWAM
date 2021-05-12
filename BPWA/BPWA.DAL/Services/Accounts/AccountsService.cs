using AutoMapper;
using BPWA.Common.Configuration;
using BPWA.Common.Resources;
using BPWA.Common.Services;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.AspNetCore.Identity;
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

        public AccountsService(
            DatabaseContext databaseContext,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ICurrentUser currentUser,
            IEmailService emailService,
            RouteSettings routeSettings,
            IUsersService usersService
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
        }

        public async Task<User> GetUserByUserNameOrEmail(string userName)
        {
            try
            {
                var user = (await UserManager.FindByNameAsync(userName)) ?? (await UserManager.FindByEmailAsync(userName));

                if (user == null)
                    throw new Exception(Translations.User_name_or_email_invalid);

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(Translations.User_name_or_email_invalid);
            }
        }

        public async Task<UserDTO> SignIn(string userName, string password)
        {
            var userResult = await GetUserByUserNameOrEmail(userName);

            var result = await SignInManager.PasswordSignInAsync(userResult, password, true, false);

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

            var userDTO = Mapper.Map<UserDTO>(userResult);

            return userDTO;
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
    }
}
