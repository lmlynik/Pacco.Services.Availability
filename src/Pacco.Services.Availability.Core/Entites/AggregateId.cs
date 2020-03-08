using System;
using System.Diagnostics.CodeAnalysis;

namespace Pacco.Services.Availability.Core.Entites
{
    public class AggregateId: IEquatable<AggregateId>
    {
        public Guid Value { get; }

        public AggregateId() : this(Guid.NewGuid())
        {
        }

        public AggregateId(Guid value) => Value = value;

        public bool Equals([AllowNull] AggregateId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Value.Equals(other.Value);
        }

        public static implicit operator Guid(AggregateId id) => id.Value;

        public static implicit operator AggregateId(Guid id) => new AggregateId(id);

        public override string ToString() => Value.ToString();
    }
}
