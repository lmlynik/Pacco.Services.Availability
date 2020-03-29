using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    public class EventProcessor : IEventProcessor
    {
        private IMessageBroker _messageBroker;
        private IEventMapper _eventMapper;
        private ILogger<IEventProcessor> _logger;
        private IServiceScopeFactory _serviceScopeFactory;

        public EventProcessor(IMessageBroker messageBroker, 
            EventMapper eventMapper,
            ILogger<IEventProcessor> logger, 
            IServiceScopeFactory serviceScopeFactory)
        {
            _messageBroker = messageBroker;
            _eventMapper = eventMapper;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ProcessAsync(IEnumerable<IDomainEvent> events)
        {
            if(events is null)
            {
                return;
            }

            var integrationEvents = HandleDomainEventsAsync(events);
            await foreach (var integrationEvent in integrationEvents)
            {
                await _messageBroker.PublishAsync(integrationEvent);
            }
        }

        private async IAsyncEnumerable<IEvent> HandleDomainEventsAsync(IEnumerable<IDomainEvent> domainEvents)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            foreach (var domainEvent in domainEvents)
            {
                var eventType = domainEvent.GetType();
                _logger.LogTrace($"Handling a domain event: { eventType.Name}");
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

                dynamic handlers = scope.ServiceProvider.GetServices(handlerType);
                foreach (var handler in handlers)
                {
                    await handler.HandleAsync(domainEvent);
                }

                var integrationEvent = _eventMapper.Map(domainEvent);
                if(integrationEvent is null)
                {
                    continue;
                }

                yield return integrationEvent;
            }
        }
    }
}
