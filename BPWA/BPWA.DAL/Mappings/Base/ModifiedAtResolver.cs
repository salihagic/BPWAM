using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using System;

namespace BPWA.DAL.Mappings
{
    public class ModifiedAtResolver : IValueResolver<IBaseAuditableEntity, IBaseAuditableDTO, DateTime?>
    {
        private ICurrentTimezone _currentTimezone;

        public ModifiedAtResolver(ICurrentTimezone currentTimezone)
        {
            _currentTimezone = currentTimezone;
        }

        public DateTime? Resolve(IBaseAuditableEntity source, IBaseAuditableDTO destination, DateTime? dateTime, ResolutionContext context)
        {
            return _currentTimezone.FromUtc(source.ModifiedAtUtc).GetValueOrDefault();
        }
    }
}
