using System;
using Pacco.Services.Availability.Core.Entites;
using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Core.Events
{
    public interface IDomainEvent
    {
    }

    public class ResourceCreated : IDomainEvent
    {
        public Resource Resource { get; set; }

        public ResourceCreated(Resource resource)
        {
            Resource = resource;
        }
    }

    public class ReservationCancelled : IDomainEvent
    {
        public Resource Resource { get; set; }
        public Reservation Reservation { get; set; }

        public ReservationCancelled(Resource resource, Reservation reservation)
        {
            Resource = resource;
            Reservation = reservation;
        }
    }
    public class ReservationAdded : IDomainEvent
    {
        public Resource Resource { get; set; }
        public Reservation Reservation { get; set; }

        public ReservationAdded(Resource resource, Reservation reservation)
        {
            Resource = resource;
            Reservation = reservation;
        }
    }
}
