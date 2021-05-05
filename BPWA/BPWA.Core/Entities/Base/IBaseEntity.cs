﻿using System;

namespace BPWA.Core.Entities
{
    public interface IBaseEntity : IBaseEntity<int> { }

    public interface IBaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? ModifiedAtUtc { get; set; }
        public DateTime? DeletedAtUtc { get; set; }
        public bool IsDeleted { get; set; }
    }
}
