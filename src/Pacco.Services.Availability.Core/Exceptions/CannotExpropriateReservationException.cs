using Pacco.Services.Availability.Core.Entites;
using System;
using System.Runtime.Serialization;

namespace Pacco.Services.Availability.Core.Exceptions
{
    [Serializable]
    public class CannotExpropriateReservationException : DomainException
    {
        public AggregateId ResourceId { get; }
        private DateTime dateTime;

        public CannotExpropriateReservationException(AggregateId id, DateTime dateTime):base($"Cannot expropriate {id}, at time {dateTime}")
        {
            this.ResourceId = id;
            this.dateTime = dateTime;
        }

        public override string Code => nameof(CannotExpropriateReservationException);

   
    }
}