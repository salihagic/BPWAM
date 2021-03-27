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
        static string superAdminRoleName = "SuperAdmin";
        static string superAdminUserName = "super.admin";
        static string companyAdminRoleName = "CompanyAdmin";
        static string companyAdminUserName = "company.admin";
        static string businessUnitAdminRoleName = "BusinessUnitAdmin";
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
                await SeedGeolocations(serviceProvider);
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
                    databaseContext.Companies.Add(new Company
                    {
                        Name = "Company X",
                        BusinessUnits = new List<BusinessUnit>
                        {
                            new BusinessUnit {
                                Name = "Business Unit X1",
                                Roles = GetBusinessUnitRoles(),
                            },
                            new BusinessUnit {
                                Name = "Business Unit X2",
                                Roles = GetBusinessUnitRoles(),
                            },
                            new BusinessUnit {
                                Name = "Business Unit X3",
                                Roles = GetBusinessUnitRoles(),
                            },
                        },
                        CompanyUsers = new List<CompanyUser>
                        {
                            new CompanyUser { UserId = companyAdminUser.Id },
                            new CompanyUser { UserId = businessUnitAdminUser.Id },
                        },
                        Roles = GetCompanyRoles()
                    });
                    databaseContext.Companies.Add(new Company
                    {
                        Name = "Company Y",
                        BusinessUnits = new List<BusinessUnit>
                        {
                            new BusinessUnit {
                                Name = "Business Unit Y1",
                                BusinessUnitUsers = new List<BusinessUnitUser>
                                {
                                    new BusinessUnitUser { UserId = companyAdminUser.Id },
                                    new BusinessUnitUser { UserId = businessUnitAdminUser.Id },
                                },
                                Roles = GetBusinessUnitRoles()
                            },
                            new BusinessUnit {
                                Name = "Business Unit Y1",
                                Roles = GetBusinessUnitRoles()
                            },
                            new BusinessUnit {
                                Name = "Business Unit Y3",
                                Roles = GetBusinessUnitRoles()
                            },
                        },
                        Roles = GetCompanyRoles()
                    });
                    databaseContext.Companies.Add(new Company
                    {
                        Name = "Company Z",
                        BusinessUnits = new List<BusinessUnit>
                        {
                            new BusinessUnit {
                                Name = "Business Unit Z1",
                                BusinessUnitUsers = new List<BusinessUnitUser>
                                {
                                    new BusinessUnitUser { UserId = companyAdminUser.Id },
                                },
                                Roles = GetBusinessUnitRoles()
                            },
                            new BusinessUnit {
                                Name = "Business Unit Z2",
                                Roles = GetBusinessUnitRoles()
                            },
                            new BusinessUnit {
                                Name = "Business Unit Z3",
                                Roles = GetBusinessUnitRoles()
                            },
                        },
                        Roles = GetCompanyRoles()
                    });
                    await databaseContext.SaveChangesAsync();
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
                    RoleClaims = new List<RoleClaim>()
                };

                await rolesService.Add(superadminRole);

                superadminRole.RoleClaims.Add(new RoleClaim
                {
                    RoleId = superadminRole.Id,
                    ClaimType = AppClaimsHelper.Authorization.Type,
                    ClaimValue = AppClaims.Authorization.GodMode
                });

                await rolesService.UpdateEntity(superadminRole);
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
                    RoleClaims = AppClaimsHelper.Authorization.Company.All.Select(x =>
                    new RoleClaim
                    {
                        ClaimType = AppClaimsHelper.Authorization.Type,
                        ClaimValue = x
                    }).ToList()
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
                    RoleClaims = AppClaimsHelper.Authorization.Company.All.Select(x =>
                    new RoleClaim
                    {
                        ClaimType = AppClaimsHelper.Authorization.Type,
                        ClaimValue = x
                    }).ToList()
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

            #endregion Super admin

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

            #endregion Company admin

            #region BusinessUnit admin

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
                        Email = "businessUnit.admin@BPWA.com"
                    }, "demo");
                }
            }

            #endregion BusinessUnit admin
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
