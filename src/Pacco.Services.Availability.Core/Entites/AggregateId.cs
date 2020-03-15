using System;
using Equatable;

namespace Pacco.Services.Availability.Core.Entites
{
    [ImplementsEquatable]
    [ToString]
    public class AggregateId
    {
        [Equals]
        public Guid Value { get; }

        public AggregateId() : this(Guid.NewGuid())
        {
        }

        public AggregateId(Guid value) => Value = value;

        public static implicit operator Guid(AggregateId id) => id.Value;

        public static implicit operator AggregateId(Guid id) => new AggregateId(id);
    }
}
