using BPWA.Common.Configuration;
using BPWA.Common.Enumerations;
using BPWA.Common.Extensions;
using BPWA.Common.Security;
using BPWA.Core.Entities;
using BPWA.DAL.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                await SeedUsers(serviceProvider);
                await SeedCompanies(serviceProvider);
                //await SeedGeolocations(serviceProvider);
                await SeedConfiguration(serviceProvider);
            }
        }

        private static async Task SeedCompanies(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();
            var companyActivityStatusLogsService = serviceProvider.GetService<ICompanyActivityStatusLogsService>();

            try
            {
                if (!databaseContext.Companies.Any())
                {
                    var superAdminUser = await databaseContext.Users
                        .IgnoreQueryFilters()
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.UserName == superAdminUserName);
                    var companyXAdminUser = await databaseContext.Users
                        .IgnoreQueryFilters()
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.UserName == companyXAdminUserName);
                    var companyYAdminUser = await databaseContext.Users
                        .IgnoreQueryFilters()
                        .Include(x => x.UserRoles)
                        .FirstOrDefaultAsync(x => x.UserName == companyYAdminUserName);

                    #region Root roles

                    var rootRoles = GetRootCompanyRoles();

                    await databaseContext.Roles.AddRangeAsync(rootRoles);
                    await databaseContext.SaveChangesAsyncWithoutCompanyId();

                    superAdminUser.UserRoles.AddRange(rootRoles.Select(x => new UserRole { RoleId = x.Id }));
                    await databaseContext.SaveChangesAsyncWithoutCompanyId();

                    #endregion

                    #region Add company (Company X)

                    var companyX = new Company
                    {
                        Name = "Company X",
                        AccountType = AccountType.Regular,
                        Roles = GetCompanyRoles(),
                        CompanyActivityStatusLogs = new List<CompanyActivityStatusLog>
                        {
                            new CompanyActivityStatusLog
                            {
                                ActivityStatus = ActivityStatus.Active
                            }
                        }
                    };

                    await databaseContext.Companies.AddAsync(companyX);

                    await databaseContext.SaveChangesAsyncWithoutCompanyId();

                    await companyActivityStatusLogsService.Add(new CompanyActivityStatusLog
                    {
                        CompanyId = companyX.Id,
                        ActivityStatus = ActivityStatus.Active
                    });

                    #endregion

                    #region Add company users (Company X)

                    companyXAdminUser.UserRoles.AddRange(
                        companyX.Roles.Select(x => new UserRole
                        {
                            RoleId = x.Id,
                            CompanyId = x.CompanyId
                        }
                    ));
                    companyXAdminUser.CompanyId = companyX.Id;
                    companyXAdminUser.CurrentCompanyId = companyX.Id;

                    await databaseContext.SaveChangesAsyncWithoutCompanyId();

                    #endregion

                    #region Add company (Company Y)

                    var companyY = new Company
                    {
                        Name = "Company Y",
                        CompanyId = companyX.Id,
                        AccountType = AccountType.Regular,
                        Roles = GetCompanyRoles(),
                        CompanyActivityStatusLogs = new List<CompanyActivityStatusLog>
                        {
                            new CompanyActivityStatusLog
                            {
                                ActivityStatus = ActivityStatus.Active
                            }
                        }
                    };

                    await databaseContext.Companies.AddAsync(companyY);
                    companyY.CompanyId = companyX.Id;

                    await databaseContext.SaveChangesAsyncWithoutCompanyId();

                    await companyActivityStatusLogsService.Add(new CompanyActivityStatusLog
                    {
                        CompanyId = companyY.Id,
                        ActivityStatus = ActivityStatus.Active
                    });

                    #endregion

                    #region Add company users (Company Y)

                    companyYAdminUser.UserRoles.AddRange(
                        companyY.Roles.Select(x => new UserRole
                        {
                            RoleId = x.Id,
                            CompanyId = x.CompanyId
                        }
                    ));
                    companyYAdminUser.CompanyId = companyY.Id;
                    companyYAdminUser.CurrentCompanyId = companyY.Id;

                    await databaseContext.SaveChangesAsyncWithoutCompanyId();

                    #endregion
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private static List<Role> GetRootCompanyRoles()
        {
            return new List<Role>
            {
                new Role
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
                }
            };
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

            var superAdminUser = await databaseContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.UserName == superAdminUserName);

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
            }

            #endregion 

            #region Company X admin

            if (environment.IsDevelopment())
            {
                var companyAdminUser = await databaseContext.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.UserName == companyXAdminUserName);

                if (companyAdminUser == null)
                {
                    var user = await usersService.AddEntity(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = companyXAdminUserName,
                        FirstName = "Company X",
                        LastName = "Admin",
                        Email = "company.x.admin@BPWA.com",
                    }, "demo");
                }
            }

            #endregion

            #region Company Y admin

            if (environment.IsDevelopment())
            {
                var subCompanyAdminUser = await databaseContext.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(x => x.UserName == companyYAdminUserName);

                if (subCompanyAdminUser == null)
                {
                    var user = await usersService.AddEntity(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = companyYAdminUserName,
                        FirstName = "Company Y",
                        LastName = "Admin",
                        Email = "company.y.admin@BPWA.com",
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

        static async Task SeedConfiguration(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            if (!databaseContext.Configuration.Any())
            {
                await databaseContext.Configuration.AddAsync(new Configuration
                {
                    ApiVersion = "1",
                    WebVersion = "1",
                    MobileVersion = "1",
                });

                await databaseContext.SaveChangesAsync();
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
