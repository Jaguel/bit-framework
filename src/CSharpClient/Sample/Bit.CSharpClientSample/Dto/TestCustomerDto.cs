﻿using Bit.Model.Contracts;
using System;

namespace Bit.Tests.Model.Dto
{
    public enum TestCustomerKind
    {
        Type1,
        Type2
    }

    public class TestCustomerDto : IDto, ISyncableDto
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }

        public virtual Guid CityId { get; set; }

        public virtual string CityName { get; set; }

        public virtual long Version { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual TestCustomerKind Kind { get; set; }

        public virtual DateTimeOffset BirthDate { get; set; }
    }
}
