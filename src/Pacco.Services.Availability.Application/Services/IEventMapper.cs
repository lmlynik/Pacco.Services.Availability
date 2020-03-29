using Convey.CQRS.Events;
using Pacco.Services.Availability.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacco.Services.Availability.Application.Services
{
    public interface IEventMapper
    {
        public IEnumerable<IEvent> MapAll(IEnumerable<IDomainEvent> events) => events.Select(Map);
        public IEvent Map(IDomainEvent @event);
    }
}
