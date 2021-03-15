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

namespace BPWA.DAL.Database
{
    public static class DatabaseSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<DatabaseContext>();

            if (databaseContext == null)
                return;

            //await databaseContext.Database.EnsureDeletedAsync();
            //await databaseContext.Database.EnsureCreatedAsync();

            try
            {
                await SeedRoles(serviceProvider);
                await SeedUsers(serviceProvider);
                await SeedGeolocations(serviceProvider);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            var rolesService = serviceProvider.GetService<IRolesService>();

            var superadminRole = await rolesService.GetEntityWithClaimsByName("Superadmin");

            if (superadminRole == null)
            {
                superadminRole = new Role
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Superadmin",
                    RoleClaims = new List<RoleClaim>()
                };

                await rolesService.Add(superadminRole);
            }

            var newRoleClaims = AppClaimsHelper.Authorization.All.Select(x => new RoleClaim
            {
                RoleId = superadminRole.Id,
                ClaimType = AppClaimsHelper.Authorization.Type,
                ClaimValue = x
            })
            .Where(x => !superadminRole.RoleClaims.Any(y => y.ClaimValue == x.ClaimValue))
            .ToList();

            superadminRole.RoleClaims.AddRange(newRoleClaims);

            await rolesService.UpdateEntity(superadminRole);
        }

        private static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var usersService = serviceProvider.GetService<IUsersService>();

            #region Superadmin

            var superadminUser = await usersService.GetEntityByUserNameOrEmail("demo.superadmin");

            if (superadminUser == null)
            {
                var user = await usersService.AddEntity(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "demo.superadmin",
                    FirstName = "Demo",
                    LastName = "Superadmin",
                    Email = "demo.superadmin@BPWA.com"
                }, "demo");

                await usersService.AddToRole(user, "Superadmin");
            }

            #endregion Superadmin
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

                    databaseContext.Currencies.AddRange(currencies);
                    databaseContext.SaveChanges();

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

                    databaseContext.Countries.AddRange(countries);
                    databaseContext.SaveChanges();
                }

                if (!databaseContext.Languages.Any())
                {
                    string jsonLanguages = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Database", "SeedData", "languages.json"));

                    var languages = JsonConvert.DeserializeObject<List<Geolocations.Language>>(jsonLanguages).Select(x => new Language
                    {
                        Code = x.code.ToUpper(),
                        Name = x.name
                    }).OrderBy(x => x.Name).ThenBy(x => x.Code);

                    databaseContext.Languages.AddRange(languages);
                    databaseContext.SaveChanges();
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
