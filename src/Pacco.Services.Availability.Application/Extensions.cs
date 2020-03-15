using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Application
{
    public static class Extensions
    {
        public static IConveyBuilder AddApplication(this IConveyBuilder builder)
            => builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}
