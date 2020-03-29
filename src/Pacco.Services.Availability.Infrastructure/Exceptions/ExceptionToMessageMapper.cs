using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Core.Exceptions;
using System;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
        => exception switch
        {
            MissingResourceTagsException ex => new AddResourceRejected(Guid.Empty, ex.Message, ex.Code),
            InvalidResourceTagsException ex => new AddResourceRejected(Guid.Empty, ex.Message, ex.Code),
            CannotExpropriateReservationException ex => new ResourceReservedRejected(ex.ResourceId, ex.Message, ex.Code),
            ResourceAlreadyExistsException ex => new AddResourceRejected(ex.ResourceId, ex.Message, ex.Code),
            ResourceNotFoundException ex => message switch
            {
                ReserveResource _ => new ResourceReservedRejected(ex.ResourceId, ex.Message, ex.Code),
                _ => null
            },
            _ => null
        };
    }
}
