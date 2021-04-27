using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using System;

namespace BPWA.DAL.Mappings
{
    public class CreatedAtResolver : IValueResolver<IBaseEntity, IBaseDTO, DateTime>
    {
        private ICurrentTimezone _currentTimezone;

        public CreatedAtResolver(ICurrentTimezone currentTimezone)
        {
            _currentTimezone = currentTimezone;
        }

        public DateTime Resolve(IBaseEntity source, IBaseDTO destination, DateTime dateTime, ResolutionContext context)
        {
            return _currentTimezone.FromUtc(source.CreatedAtUtc).GetValueOrDefault();
        }
    }
}
