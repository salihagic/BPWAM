using AutoMapper;
using BPWA.Common.Exceptions;
using BPWA.Common.Resources;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.DAL.Services
{
    public class UsersService : IUsersService
    {
        protected readonly IMapper Mapper;
        protected readonly DatabaseContext DatabaseContext;
        protected readonly UserManager<User> UserManager;
        protected readonly SignInManager<User> SignInManager;
        protected readonly CurrentUser CurrentUser;

        public UsersService(
            DatabaseContext databaseContext,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            CurrentUser loggedUserService
            )
        {
            Mapper = mapper;
            DatabaseContext = databaseContext;
            UserManager = userManager;
            SignInManager = signInManager;
            CurrentUser = loggedUserService;
        }

        public Task<UserDTO> Add(User entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> AddEntity(User entity, string password)
        {
            var result = await UserManager.CreateAsync(entity, password);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            return entity;
        }

        public Task<User> AddEntity(User entity)
        {
            throw new System.NotImplementedException();
        }

        public async Task<UserDTO> AddToRole(User entity, string roleName)
        {
            var result = await UserManager.AddToRoleAsync(entity, roleName);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            return Mapper.Map<UserDTO>(entity);
        }

        public Task Delete(User entity, bool softDelete = true)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string id, bool softDelete = true)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<UserDTO>> Get()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserDTO> GetById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetEntities()
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetEntityById(string id)
        {
            return await DatabaseContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetEntityByUserNameOrEmail(string userName)
        {
            return (await UserManager.FindByNameAsync(userName)) ?? (await UserManager.FindByEmailAsync(userName));
        }

        public async Task<UserDTO> SignIn(string userName, string password)
        {
            var user = await GetEntityByUserNameOrEmail(userName);

            if (user == null)
                throw new ValidationException(Translations.User_name_or_email_invalid);

            var result = await SignInManager.PasswordSignInAsync(user, password, true, false);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    throw new ValidationException(Translations.Account_locked_out);
                else if (result.IsNotAllowed)
                    throw new ValidationException(Translations.Login_not_allowed);
                else if (result.RequiresTwoFactor)
                    throw new ValidationException(Translations.Login_required_two_factor);
                else
                    throw new ValidationException(Translations.User_name_or_email_invalid);
            }

            return Mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> Update(User entity)
        {
            return Mapper.Map<UserDTO>(await UpdateEntity(entity));
        }

        public async Task<User> UpdateEntity(User entity)
        {
            DatabaseContext.Users.Update(entity);
            await DatabaseContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateTimezoneForLoggedUser(int timezoneUtcOffsetInMinutes)
        {
            var loggedUserId = CurrentUser.Id();

            if (loggedUserId != null)
            {
                var user = await GetEntityById(loggedUserId);

                if (user != null)
                {
                    var timezoneInfo = TimeZoneInfo.GetSystemTimeZones()
                                                   .Where(x => x.BaseUtcOffset == (new TimeSpan(0, timezoneUtcOffsetInMinutes, 0)))
                                                   .FirstOrDefault();

                    if (timezoneInfo != null)
                    {
                        user.TimezoneId = timezoneInfo.Id;
                        await Update(user);
                    }
                }
            }
        }
    }
}
