using Convey.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Documents
{
    internal sealed class ResourceDocument: IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public uint Version { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<ReservationDocument> Reservations { get; set; }
    }

    internal sealed class ReservationDocument
    {
        public int Timestamp { get; set; }
        public int Priority { get; set; }
    }
}
