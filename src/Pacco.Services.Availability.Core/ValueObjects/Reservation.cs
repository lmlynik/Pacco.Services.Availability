using System;
using Equatable;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    [ImplementsEquatable]
    [ToString]
    public class Reservation
    {
        [Equals]
        public DateTime DateTime { get; }
        [Equals]
        public int Priority { get; }

        public Reservation(DateTime date, int priority)
        {
            DateTime = date;
            Priority = priority;
        }
    }
}
