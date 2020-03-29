using Convey.CQRS.Events;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    public class EventMapper : IEventMapper
    {
        public IEvent Map(IDomainEvent @event)
         => @event switch
         {
             ResourceCreated e => new ResourceAdded(e.Resource.Id),
             ReservationCancelled e => new ResourceReservationCancelled(e.Resource.Id, e.Reservation.DateTime),
             ReservationAdded e => new ResourceReserved(e.Resource.Id, e.Reservation.DateTime),
             _ => null
         };
    }
}
