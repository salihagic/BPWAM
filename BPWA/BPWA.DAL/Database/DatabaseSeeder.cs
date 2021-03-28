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

namespace BPWA.DAL.Database
{
    public static class DatabaseSeeder
    {
        static string superAdminRoleName = "Super admin";
        static string superAdminUserName = "super.admin";
        static string companyAdminRoleName = "Company admin";
        static string companyAdminUserName = "company.admin";
        static string businessUnitAdminRoleName = "Business unit admin";
        static string businessUnitAdminUserName = "businessunit.admin";

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
                    await SeedCompaniesAndBusinessUnits(serviceProvider);
                //await SeedGeolocations(serviceProvider);
            }
        }

        private static async Task SeedCompaniesAndBusinessUnits(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();

            var superAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == superAdminUserName);
            var companyAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == companyAdminUserName);
            var businessUnitAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == businessUnitAdminUserName);

            try
            {
                if (!databaseContext.Companies.Any())
                {
                    /*
                                          super.admin     company.admin                 businessunit.admin       
                    Company X             GodMode         User, Company GodMode         -
                    Business unit X1      GodMode         -                             User, Business unit GodMode
                    Business unit X2      GodMode         -                             -
                    Business unit X3      GodMode         -                             -
                    Company Y             GodMode         -                             -
                    Business unit Y1      GodMode         User, Business unit GodMode   User, Business unit GodMode 
                    Business unit Y2      GodMode         User                          User
                    Business unit Y3      GodMode         User                          User
                    Company Z             GodMode         -                             -
                    Business unit Z1      GodMode         -                             -
                    Business unit Z2      GodMode         -                             -
                    Business unit Z3      GodMode         -                             -                    
                    */

                    #region Add company (Company X)

                    var companyX = new Company
                    {
                        Name = "Company X",
                        Roles = GetCompanyRoles()
                    };

                    await databaseContext.Companies.AddAsync(companyX);

                    await databaseContext.SaveChangesAsync();

                    #region Add company users (Company X)

                    var companyXCompanyAdminRole = companyX.Roles.FirstOrDefault(x => x.Name == companyAdminRoleName);

                    var companyXUser1 = new CompanyUser
                    {
                        UserId = companyAdminUser.Id,
                        CompanyUserRoles = new List<CompanyUserRole>
                        {
                            new CompanyUserRole { RoleId = companyXCompanyAdminRole.Id }
                        }
                    };

                    companyX.CompanyUsers = new List<CompanyUser>() { companyXUser1 };

                    companyAdminUser.CurrentCompanyId = companyX.Id;

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add business units (Company X)

                    #region Add business unit (Business unit X1)

                    var businessUnitX1 = new BusinessUnit
                    {
                        Name = "Business Unit X1",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyX.BusinessUnits = new List<BusinessUnit> { businessUnitX1 };

                    await databaseContext.SaveChangesAsync();

                    #region Add business unit users (Business unit X1)

                    var businessUnitX1BusinessUnitAdminRole = businessUnitX1.Roles.FirstOrDefault(x => x.Name == businessUnitAdminRoleName);

                    var businessUnitX1User1 = new BusinessUnitUser
                    {
                        UserId = businessUnitAdminUser.Id,
                        BusinessUnitUserRoles = new List<BusinessUnitUserRole>
                        {
                            new BusinessUnitUserRole { RoleId = businessUnitX1BusinessUnitAdminRole.Id }
                        }
                    };

                    businessUnitX1.BusinessUnitUsers = new List<BusinessUnitUser>() { businessUnitX1User1 };

                    businessUnitAdminUser.CurrentCompanyId= businessUnitX1.CompanyId;
                    businessUnitAdminUser.CurrentBusinessUnitId = businessUnitX1.Id;

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #endregion

                    #region Add business unit (Business unit X2)

                    var businessUnitX2 = new BusinessUnit
                    {
                        Name = "Business Unit X2",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyX.BusinessUnits.Add(businessUnitX2);

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add business unit (Business unit X3)

                    var businessUnitX3 = new BusinessUnit
                    {
                        Name = "Business Unit X3",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyX.BusinessUnits.Add(businessUnitX3);

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #endregion

                    #endregion

                    #region Add company (Company Y)

                    var companyY = new Company
                    {
                        Name = "Company Y",
                        Roles = GetCompanyRoles()
                    };

                    await databaseContext.Companies.AddAsync(companyY);

                    await databaseContext.SaveChangesAsync();

                    #region Add business units (Company Y)

                    #region Add business unit (Business unit Y1)

                    var businessUnitY1 = new BusinessUnit
                    {
                        Name = "Business Unit Y1",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyY.BusinessUnits = new List<BusinessUnit> { businessUnitY1 };

                    await databaseContext.SaveChangesAsync();

                    #region Add business unit users (Business unit Y1)

                    var businessUnitY1BusinessUnitAdminRole = businessUnitY1.Roles.FirstOrDefault(x => x.Name == businessUnitAdminRoleName);

                    var businessUnitY1User1 = new BusinessUnitUser
                    {
                        UserId = companyAdminUser.Id,
                        BusinessUnitUserRoles = new List<BusinessUnitUserRole>
                        {
                            new BusinessUnitUserRole { RoleId = businessUnitY1BusinessUnitAdminRole.Id }
                        }
                    };
                    var businessUnitY1User2 = new BusinessUnitUser
                    {
                        UserId = businessUnitAdminUser.Id,
                        BusinessUnitUserRoles = new List<BusinessUnitUserRole>
                        {
                            new BusinessUnitUserRole { RoleId = businessUnitY1BusinessUnitAdminRole.Id }
                        }
                    };

                    businessUnitY1.BusinessUnitUsers = new List<BusinessUnitUser>() 
                    { 
                        businessUnitY1User1,
                        businessUnitY1User2,
                    };

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #endregion

                    #region Add business unit (Business unit Y2)

                    var businessUnitY2 = new BusinessUnit
                    {
                        Name = "Business Unit Y2",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyY.BusinessUnits.Add(businessUnitY2);

                    await databaseContext.SaveChangesAsync();

                    #region Add business unit users (Business unit Y2)

                    var businessUnitY2User1 = new BusinessUnitUser
                    {
                        UserId = companyAdminUser.Id,
                    };

                    businessUnitY2.BusinessUnitUsers = new List<BusinessUnitUser>() { businessUnitY2User1 };

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #endregion

                    #region Add business unit (Business unit Y3)

                    var businessUnitY3 = new BusinessUnit
                    {
                        Name = "Business Unit Y3",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyY.BusinessUnits.Add(businessUnitY3);

                    await databaseContext.SaveChangesAsync();

                    #region Add business unit users (Business unit Y3)

                    var businessUnitY3User1 = new BusinessUnitUser
                    {
                        UserId = companyAdminUser.Id,
                    };

                    businessUnitY3.BusinessUnitUsers = new List<BusinessUnitUser>() { businessUnitY3User1 };

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #endregion

                    #endregion

                    #endregion

                    #region Add company (Company Z)

                    var companyZ = new Company
                    {
                        Name = "Company Z",
                        Roles = GetCompanyRoles()
                    };

                    await databaseContext.Companies.AddAsync(companyZ);

                    await databaseContext.SaveChangesAsync();

                    #region Add business units (Company Z)

                    #region Add business unit (Business unit Z1)

                    var businessUnitZ1 = new BusinessUnit
                    {
                        Name = "Business Unit Z1",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyZ.BusinessUnits = new List<BusinessUnit> { businessUnitZ1 };

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add business unit (Business unit Z2)

                    var businessUnitZ2 = new BusinessUnit
                    {
                        Name = "Business Unit Z2",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyZ.BusinessUnits.Add(businessUnitZ2);

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #region Add business unit (Business unit Z3)

                    var businessUnitZ3 = new BusinessUnit
                    {
                        Name = "Business Unit Z3",
                        Roles = GetBusinessUnitRoles(),
                    };

                    companyZ.BusinessUnits.Add(businessUnitZ3);

                    await databaseContext.SaveChangesAsync();

                    #endregion

                    #endregion

                    #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var rolesService = serviceProvider.GetService<IRolesService>();

            #region Superadmin

            var superadminRole = await rolesService.GetEntityWithClaimsByName(superAdminRoleName);

            if (superadminRole == null)
            {
                superadminRole = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = superAdminRoleName,
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
                    Name = companyAdminRoleName,
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

        private static List<Role> GetBusinessUnitRoles()
        {
            return new List<Role>
            {
                new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = businessUnitAdminRoleName,
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim
                        {
                            ClaimType = AppClaimsHelper.Authorization.Type,
                            ClaimValue = AppClaims.Authorization.BusinessUnit.BusinessUnitGodMode
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

                await usersService.AddToRole(user.Item, superAdminRoleName);
            }

            #endregion 

            #region Company admin

            if (environment.IsDevelopment())
            {
                var companyAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == companyAdminUserName);

                if (companyAdminUser == null)
                {
                    var user = await usersService.AddEntity(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = companyAdminUserName,
                        FirstName = "Company",
                        LastName = "Admin",
                        Email = "company.admin@BPWA.com",
                    }, "demo");
                }
            }

            #endregion 

            #region Business unit admin

            if (environment.IsDevelopment())
            {
                var businessUnitAdminUser = await databaseContext.Users.FirstOrDefaultAsync(x => x.UserName == businessUnitAdminUserName);

                if (businessUnitAdminUser == null)
                {
                    var user = await usersService.AddEntity(new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = businessUnitAdminUserName,
                        FirstName = "BusinessUnit",
                        LastName = "Admin",
                        Email = "businessunit.admin@BPWA.com"
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
