using AutoMapper;
using BPWA.Core.Entities;
using BPWA.DAL.Models;
using BPWA.DAL.Services;
using System;

namespace BPWA.DAL.Mappings
{
    public class DeletedAtResolver : IValueResolver<IBaseAuditableEntity, IBaseAuditableDTO, DateTime?>
    {
        private ICurrentTimezone _currentTimezone;

        public DeletedAtResolver(ICurrentTimezone currentTimezone)
        {
            _currentTimezone = currentTimezone;
        }

        public DateTime? Resolve(IBaseAuditableEntity source, IBaseAuditableDTO destination, DateTime? dateTime, ResolutionContext context)
        {
            return _currentTimezone.FromUtc(source.DeletedAtUtc).GetValueOrDefault();
        }
    }
}
