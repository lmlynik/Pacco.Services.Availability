using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Availability.Core.Events;

namespace Pacco.Services.Availability.Core.Entites
{
    public class AggregateRoot
    {
        private readonly ISet<IDomainEvent> _events = new HashSet<IDomainEvent>();
        public IEnumerable<IDomainEvent> Events => _events;
        public AggregateId Id { get; protected set; }
        public uint Version { get; protected set; }

        protected void AddEvent(IDomainEvent @event)
        {
            if (!_events.Any())
            {
                Version++;
            }

            _events.Add(@event);
        }

        public void ClearEvents() => _events.Clear();

        public AggregateRoot()
        {
        }
    }
}
