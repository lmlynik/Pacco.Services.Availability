using Pacco.Services.Availability.Core.Entites;
using System;
using System.Runtime.Serialization;

namespace Pacco.Services.Availability.Core.Exceptions
{
    [Serializable]
    public class CannotExpropriateReservationException : DomainException
    {
        private AggregateId id;
        private DateTime dateTime;

        public CannotExpropriateReservationException(AggregateId id, DateTime dateTime):base($"Cannot expropriate {id}, at time {dateTime}")
        {
            this.id = id;
            this.dateTime = dateTime;
        }

        public override string Code => nameof(CannotExpropriateReservationException);
    }
}