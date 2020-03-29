using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Pacco.Services.Availability.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private readonly IBusPublisher _busPublisher;

        public MessageBroker(IBusPublisher busPublisher)
        {
            this._busPublisher = busPublisher;
        }

        public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            if (events is null)
            {
                return;
            }

            foreach (var @event in events)
            {
                if(@event is null)
                {
                    continue;
                }

                await _busPublisher.PublishAsync(@event);
            }
        }
    }
}
