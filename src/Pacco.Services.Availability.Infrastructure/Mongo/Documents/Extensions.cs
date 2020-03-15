using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Core.Entites;
using Pacco.Services.Availability.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static Resource AsEntity(this ResourceDocument document)
         => new Resource(document.Id,
             document.Tags,
             document.Reservations.Select(r => new Reservation(r.Timestamp.AsDateTime(), r.Priority)),
             document.Version);


        public static ResourceDocument AsDocument(this Resource resource)
            => new ResourceDocument
            {
                Id = resource.Id,
                Version = resource.Version,
                Tags = resource.Tags,
                Reservations = resource.Reservations.Select(r => new ReservationDocument
                {
                    Timestamp = r.DateTime.AsDaysSinceEpoch(),
                    Priority = r.Priority,
                })
            };

        public static ResourceDto AsDto(this ResourceDocument document)
            => new ResourceDto
            {
                Id = document.Id,
                Reservations = document.Reservations.Select(r => new ReservationDto
                {
                    DateTime = r.Timestamp.AsDateTime(),
                    Priority = r.Priority
                }),
                Tags = document.Tags
            };

        internal static int AsDaysSinceEpoch(this DateTime dateTime) => (dateTime - new DateTime()).Days;

        internal static DateTime AsDateTime(this int daysSinceEpoch) => new DateTime().AddDays(daysSinceEpoch);

    }



}
