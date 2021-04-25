using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using BPWA.Web.Services.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPWA.Web.Services.Services
{
    public class CountriesWebService : CountriesService, ICountriesWebService
    {
        public CountriesWebService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper)
        {
        }

        public override IQueryable<Country> BuildIncludesById(int id, IQueryable<Country> query)
        {
            return base.BuildIncludesById(id, query)
                       .Include(x => x.CountryCurrencies)
                       .ThenInclude(x => x.Currency)
                       .Include(x => x.CountryLanguages)
                       .ThenInclude(x => x.Language);
        }

        public async Task<Result<CountryAddModel>> PrepareForAdd(CountryAddModel model = null)
        {
            model ??= new CountryAddModel();

            if (model.CurrencyIds.IsNotEmpty())
            {
                try
                {
                    model.CurrencyIdsSelectList = await DatabaseContext.Currencies
                    .Where(x => model.CurrencyIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<CountryAddModel>("Could not load currencies");
                }
            }
            if (model.LanguageIds.IsNotEmpty())
            {
                try
                {
                    model.LanguageIdsSelectList = await DatabaseContext.Languages
                    .Where(x => model.LanguageIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<CountryAddModel>("Could not load languages");
                }
            }

            model.CurrencyIdsSelectList ??= new List<SelectListItem>();
            model.LanguageIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public async Task<Result<CountryUpdateModel>> PrepareForUpdate(CountryUpdateModel model = null)
        {
            model ??= new CountryUpdateModel();

            if (model.CurrencyIds.IsNotEmpty())
            {
                try
                {
                    model.CurrencyIdsSelectList = await DatabaseContext.Currencies
                    .Where(x => model.CurrencyIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<CountryUpdateModel>("Could not load currencies");
                }
            }
            if (model.LanguageIds.IsNotEmpty())
            {
                try
                {
                    model.LanguageIdsSelectList = await DatabaseContext.Languages
                    .Where(x => model.LanguageIds.Contains(x.Id))
                    .Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToListAsync();
                }
                catch (Exception e)
                {
                    return Result.Failed<CountryUpdateModel>("Could not load languages");
                }
            }

            model.CurrencyIdsSelectList ??= new List<SelectListItem>();
            model.LanguageIdsSelectList ??= new List<SelectListItem>();

            return Result.Success(model);
        }

        public override async Task<Result<CountryDTO>> Update(Country entity)
        {
            var currentCountryCurrencies = await DatabaseContext.CountryCurrencies.Where(x => x.CountryId == entity.Id).ToListAsync();

            if (currentCountryCurrencies.IsNotEmpty())
            {
                //Delete
                var countryCurrenciesToDelete = currentCountryCurrencies.Where(x => !entity.CountryCurrencies?.Any(y => y.CurrencyId == x.CurrencyId) ?? true).ToList();
                DatabaseContext.RemoveRange(countryCurrenciesToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.CountryCurrencies = entity.CountryCurrencies.Where(x => !currentCountryCurrencies.Any(y => y.CurrencyId == x.CurrencyId)).ToList();
            }

            var currentCountryLanguages = await DatabaseContext.CountryLanguages.Where(x => x.CountryId == entity.Id).ToListAsync();

            if (currentCountryLanguages.IsNotEmpty())
            {
                //Delete
                var countryLanguagesToDelete = currentCountryLanguages.Where(x => !entity.CountryLanguages?.Any(y => y.LanguageId == x.LanguageId) ?? true).ToList();
                DatabaseContext.RemoveRange(countryLanguagesToDelete);
                await DatabaseContext.SaveChangesAsync();

                //Only leave the new ones
                entity.CountryLanguages = entity.CountryLanguages.Where(x => !currentCountryLanguages.Any(y => y.LanguageId == x.LanguageId)).ToList();
            }

            return await base.Update(entity);
        }
    }
}
