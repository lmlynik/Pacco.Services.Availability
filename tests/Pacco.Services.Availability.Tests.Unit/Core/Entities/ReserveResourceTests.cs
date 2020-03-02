using System;
using System.Drawing;
using System.Linq;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.Exceptions;
using Pacco.Services.Availability.Core.ValueObjects;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Core.Entities
{
    public class ReserveResourceTests
    {
        void act(Resource resource, Reservation reservation)
            => resource.AddReservation(reservation);

        [Fact]
        public void given_reservation_with_no_collision_reservation_should_be_added_to_resource()
        {
            var dateTime = DateTime.Now;
            var priority = 1;
            
            var resource = new Resource(new AggregateId(), new[] {"tags"});
            var reservation = new Reservation(dateTime, priority);
            
            act(resource, reservation);
            
            var addedReservation = resource.Reservations.FirstOrDefault();
            
            addedReservation.ShouldNotBeNull();
            addedReservation.DateTime.ShouldBe(dateTime);
            addedReservation.Priority.ShouldBe(priority);
            
            resource.Events.Count().ShouldBe(1);
            resource.Events.Last().ShouldBeOfType<ReservationAdded>();
        }
        
        [Fact]
        public void given_reservation_with_higher_priority_than_colliding_one_reservation_should_be_added_to_resource()
        {
            var dateTime = DateTime.Now;
            var priority = 1;
            var collidingReservation = new Reservation(DateTime.Now, 0);
            
            var resource = new Resource(new AggregateId(), new[] {"tags"}, new [] {collidingReservation});
            var reservation = new Reservation(dateTime, priority);
            
            act(resource, reservation);
            
            var addedReservation = resource.Reservations.FirstOrDefault();
            
            addedReservation.ShouldNotBeNull();
            addedReservation.DateTime.ShouldBe(dateTime);
            addedReservation.Priority.ShouldBe(priority);
            
            resource.Events.Count().ShouldBe(2);
            resource.Events.First().ShouldBeOfType<ReservationCanceled>();
            resource.Events.Last().ShouldBeOfType<ReservationAdded>();
        }
        
        [Fact]
        public void given_reservation_with_lower_priority_than_colliding_one_resource_should_throw_CannotExpropriateReservationException()
        {
            var dateTime = DateTime.Now;
            var priority = 0;
            var collidingReservation = new Reservation(DateTime.Now, 1);
            
            var resource = new Resource(new AggregateId(), new[] {"tags"}, new [] {collidingReservation});
            var reservation = new Reservation(dateTime, priority);
            
            var exception = Record.Exception(() => act(resource, reservation));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CannotExpropriateReservationException>();
        }
    }
}