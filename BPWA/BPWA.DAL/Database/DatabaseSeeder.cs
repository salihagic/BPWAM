using BPWA.Common.Configuration;
using BPWA.Common.Extensions;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using BPWA.DAL.Models;

namespace BPWA.DAL.Database
{
    public static class DatabaseSeeder
    {
        static string superAdminRoleName = "Super admin";
        static string adminRoleName = "Company admin";

        //Password is demo
        static string superAdminUserName = "super.admin";
        //Password is demo
        static string companyXAdminUserName = "company.x.admin";
        //Password is demo
        static string companyYAdminUserName = "company.y.admin";

        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();
            var databaseSettings = serviceProvider.GetService<DatabaseSettings>();
            var environment = serviceProvider.GetService<IHostingEnvironment>();

            if (databaseSettings.RecreateDatabase)
            {
                await databaseContext.Database.EnsureDeletedAsync();
                await databaseContext.Database.EnsureCreatedAsync();
            }
            if (databaseSettings.AutoMigrate)
            {
                await databaseContext.Database.MigrateAsync();
            }
            if (databaseSettings.Seed)
            {
                await SeedRoles(serviceProvider);
                await SeedUsers(serviceProvider);
                if (environment.IsDevelopment())
                    await SeedCompanies(serviceProvider);
                //await SeedGeolocations(serviceProvider);
            }
        }

        private static async Task SeedCompanies(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();

            try
            {
                if (!databaseContext.Companies.Any())
                {
                    var superAdminUser = await databaseContext.Users
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.UserName == superAdminUserName);
                    var companyXAdminUser = await databaseContext.Users
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.UserName == companyXAdminUserName);
                    var companyYAdminUser = await databaseContext.Users
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.UserName == companyYAdminUserName);

                    #region Add company (Company X)

                    var companyX = new Company
                    {
                        Name = "Company X",
                        Roles = GetCompanyRoles()
                    };

                    await databaseContext.Companies.AddAsync(companyX);

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add company users (Company X)

                    var companyXAdminRole = companyX.Roles.FirstOrDefault(x => x.Name == adminRoleName);

                    companyXAdminUser.UserRoles.Add(new UserRole { RoleId = companyXAdminRole.Id });

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add company (Company Y)

                    var companyY = new Company
                    {
                        Name = "Company Y",
                        Roles = GetCompanyRoles()
                    };

                    await databaseContext.Companies.AddAsync(companyY);

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add company users (Company Y)

                    var companyYAdminRole = companyY.Roles.FirstOrDefault(x => x.Name == adminRoleName);

                    companyYAdminUser.UserRoles.Add(new UserRole { RoleId = companyYAdminRole.Id });

                    await databaseContext.SaveChangesAsync();

                    #endregion
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var rolesService = serviceProvider.GetService<IRolesService>();

            #region Superadmin

            var superadminRole = (await rolesService.GetEntities(new RoleSearchModel { Name = superAdminRoleName }))?.FirstOrDefault();

            if (superadminRole == null)
            {
                superadminRole = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = superAdminRoleName,
                    NormalizedName = superAdminRoleName.ToUpper(),
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim
                        {
                            ClaimType = AppClaimsHelper.Authorization.Type,
                            ClaimValue = AppClaims.Authorization.Administration.GodMode
                        }
                    }
                };

                await rolesService.Add(superadminRole);
            }

            #endregion Superadmin
        }

        private static List<Role> GetCompanyRoles()
        {
            return new List<Role>
            {
                new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = adminRoleName,
                    NormalizedName = adminRoleName.ToUpper(),
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim
                        {
                            ClaimType = AppClaimsHelper.Authorization.Type,
                            ClaimValue = AppClaims.Authorization.Company.CompanyGodMode
                        }
                    }
                }
            };
        }

        private static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();
            var usersService = serviceProvider.GetService<IUsersService>();
            var environment = serviceProvider.GetService<IHostingEnvironment>();

            #region Super admin

            var superAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == superAdminUserName);

            if (superAdminUser == null)
            {
                var user = await usersService.AddEntity(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = superAdminUserName,
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "super.admin@BPWA.com"
                }, "demo");

                await usersService.AddToRole(user, superAdminRoleName);
            }

            #endregion 

            #region Company admin

            if (environment.IsDevelopment())
            {
                var companyAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == companyXAdminUserName);

                if (companyAdminUser == null)
                {
                    var user = await usersService.AddEntity(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = companyXAdminUserName,
                        FirstName = "Company",
                        LastName = "Admin",
                        Email = "company.admin@BPWA.com",
                    }, "demo");
                }
            }

            #endregion

            #region Subcompany admin

            if (environment.IsDevelopment())
            {
                var subCompanyAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == companyYAdminUserName);

                if (subCompanyAdminUser == null)
                {
                    var user = await usersService.AddEntity(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = companyYAdminUserName,
                        FirstName = "Subcompany",
                        LastName = "Admin",
                        Email = "sub.company.admin@BPWA.com",
                    }, "demo");
                }
            }

            #endregion 
        }

        public static async Task SeedGeolocations(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();

            try
            {
                if (!databaseContext.Countries.Any())
                {
                    string jsonCountries = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Database", "SeedData", "countries.json"));

                    var countries = JsonConvert.DeserializeObject<List<Geolocations.Country>>(jsonCountries).Select(x => new Country
                    {
                        Code = x.code.ToUpper(),
                        Name = x.name,
                        CountryCurrencies = x.primary_currency != null ? new List<CountryCurrency>
                        {
                            new CountryCurrency
                            {
                                Currency = new Currency
                                {
                                    Code = x.primary_currency.code,
                                    Name = x.primary_currency.name,
                                    Symbol = x.primary_currency.symbol
                                }
                            }
                        } : null,
                        Cities = x.cities?.Select(y => new City
                        {
                            Name = y.name,
                            Latitude = y.latitude,
                            Longitude = y.longitude
                        })?.ToList()
                    }).OrderBy(x => x.Name).ThenBy(x => x.Code).ToList();

                    var currencies = countries.Where(x => x.CountryCurrencies != null)
                                              .SelectMany(y => y.CountryCurrencies.Where(z => z.Currency != null)
                                              .Select(z => z.Currency))
                                              .GroupBy(x => new
                                              {
                                                  x.Code,
                                                  x.Symbol,
                                                  x.Name,
                                              })
                                              .Select(x => new Currency
                                              {
                                                  Code = x.Key.Code,
                                                  Symbol = x.Key.Symbol,
                                                  Name = x.Key.Name,
                                              }).ToList();

                    await databaseContext.Currencies.AddRangeAsync(currencies);
                    await databaseContext.SaveChangesAsync();

                    countries.ForEach(x =>
                    {
                        if (x.CountryCurrencies.IsNotEmpty())
                        {
                            x.CountryCurrencies = currencies.Where(y => x.CountryCurrencies.Any(z => z.Currency.Code == y.Code))
                            .Select(y => new CountryCurrency
                            {
                                CountryId = x.Id,
                                CurrencyId = y.Id
                            }).ToList();
                        }
                    });

                    await databaseContext.Countries.AddRangeAsync(countries);
                    await databaseContext.SaveChangesAsync();
                }

                if (!databaseContext.Languages.Any())
                {
                    string jsonLanguages = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Database", "SeedData", "languages.json"));

                    var languages = JsonConvert.DeserializeObject<List<Geolocations.Language>>(jsonLanguages).Select(x => new Language
                    {
                        Code = x.code.ToUpper(),
                        Name = x.name
                    }).OrderBy(x => x.Name).ThenBy(x => x.Code);

                    await databaseContext.Languages.AddRangeAsync(languages);
                    await databaseContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {

            }
        }

        public class Geolocations
        {
            public class Country
            {
                public string code { get; set; }
                public string name { get; set; }
                public Currency primary_currency { get; set; }
                public List<City> cities { get; set; }
            }

            public class City
            {
                public string name { get; set; }
                public double latitude { get; set; }
                public double longitude { get; set; }
            }

            public class Currency
            {
                public string code { get; set; }
                public string name { get; set; }
                public string symbol { get; set; }
            }

            public class Language
            {
                public string code { get; set; }
                public string name { get; set; }
            }
        }
    }
}
