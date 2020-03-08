using System;
using System.Diagnostics.CodeAnalysis;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    public struct Reservation: IEquatable<Reservation>
    {
        public DateTime DateTime { get; }
        public int Priority { get; }

        public Reservation(DateTime date, int priority)
        {
            DateTime = date;
            Priority = priority;
        }

        public override bool Equals(object obj) => obj is Reservation reservation && Equals(reservation);

        public bool Equals([AllowNull] Reservation other) => DateTime == other.DateTime && Priority == other.Priority;

        public override int GetHashCode() => HashCode.Combine(DateTime, Priority);
    }
}
