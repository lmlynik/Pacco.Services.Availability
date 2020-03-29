using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;
using System.Threading.Tasks;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public class ReserveResourceHandler : ICommandHandler<ReserveResource>
    {
        private readonly IResourcesRepository resorceRepository;
        private readonly IEventProcessor _eventProcessor;

        public ReserveResourceHandler(IResourcesRepository resorceRepository, IEventProcessor eventProcessor)
        {
            this.resorceRepository = resorceRepository;
            _eventProcessor = eventProcessor;
        }

        public async Task HandleAsync(ReserveResource command)
        {
            var resource = await resorceRepository.GetAsync(command.ResourceId);
            if (resource is null)
            {
                throw new ResourceNotFoundException(command.ResourceId);
            }

            var reservation = new Reservation(command.DateTime, command.Priority);
            resource.AddReservation(reservation);
            await resorceRepository.UpdateAsync(resource);
            await _eventProcessor.ProcessAsync(resource.Events);
        }
    }
}
