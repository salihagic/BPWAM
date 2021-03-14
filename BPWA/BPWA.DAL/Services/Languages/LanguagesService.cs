﻿using AutoMapper;
using BPWA.Common.Extensions;
using BPWA.Core.Entities;
using BPWA.DAL.Database;
using BPWA.DAL.Models;
using System.Linq;

namespace BPWA.DAL.Services
{
    public class LanguagesService : BaseService<Language, LanguageSearchModel, LanguageDTO>, ILanguagesService
    {
        public LanguagesService(
            DatabaseContext databaseContext,
            IMapper mapper
            ) : base(databaseContext, mapper) { }

        public override IQueryable<Language> BuildQueryConditions(IQueryable<Language> query, LanguageSearchModel searchModel = null)
        {
            return base.BuildQueryConditions(query, searchModel)
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Name), x => x.Name.ToLower().StartsWith(searchModel.Name.ToLower()))
                       .WhereIf(!string.IsNullOrEmpty(searchModel.Code), x => x.Code.ToLower().StartsWith(searchModel.Code.ToLower()));
        }
    }
}
