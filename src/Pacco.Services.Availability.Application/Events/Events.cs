using Convey.CQRS.Events;
using System;

namespace Pacco.Services.Availability.Application.Events
{
    [Contract]
    public class ResourceAdded : IEvent
    {
        public Guid ResourceId { get; }

        public ResourceAdded(Guid resourceId)
        {
            ResourceId = resourceId;
        }
    }

    [Contract]
    public class ResourceReserved : IEvent
    {
        public Guid ResourceId { get; }
        public DateTime DateTime { get; }

        public ResourceReserved(Guid resourceId, DateTime dateTime)
        {
            ResourceId = resourceId;
            DateTime = dateTime;
        }
    }

    [Contract]
    public class AddResourceRejected : IRejectedEvent
    {
        public Guid ResourceId { get; }

        public string Reason { get; }

        public string Code { get; }

        public AddResourceRejected(Guid resourceId, string reason, string code)
        {
            ResourceId = resourceId;
            Reason = reason;
            Code = code;
        }
    }

    [Contract]
    public class ResourceReservedRejected : IRejectedEvent
    {
        public Guid ResourceId { get; }
        public string Reason { get; }
        public string Code { get; }
        public ResourceReservedRejected(Guid resourceId, string reason, string code)
        {
            ResourceId = resourceId;
            Reason = reason;
            Code = code;
        }
    }

    [Contract]
    public class ResourceReservationCancelled : IEvent
    {
        public Guid ResourceId { get; }
        public DateTime DateTime { get; }

        public ResourceReservationCancelled(Guid resourceId, DateTime dateTime)
        {
            ResourceId = resourceId;
            DateTime = dateTime;
        }
    }


}
