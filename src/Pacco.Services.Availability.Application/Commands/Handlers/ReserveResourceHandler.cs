using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Core.ValueObjects;
using System.Threading.Tasks;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public class ReserveResourceHandler : ICommandHandler<ReserveResource>
    {
        private readonly IResourcesRepository resorceRepository;

        public ReserveResourceHandler(IResourcesRepository resorceRepository)
        {
            this.resorceRepository = resorceRepository;
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

        }
    }
}
