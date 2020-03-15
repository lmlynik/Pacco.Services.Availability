using Convey.CQRS.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Core.Entites;
using Pacco.Services.Availability.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pacco.Services.Availability.Application.Commands.Handlers
{
    public class AddResourceHandler : ICommandHandler<AddResource>
    {
        private readonly IResourcesRepository _resourcesRepository;

        public AddResourceHandler(IResourcesRepository resourcesRepository)
        {
            _resourcesRepository = resourcesRepository;
        }

        public async Task HandleAsync(AddResource command)
        {
            if (await _resourcesRepository.ExistsAsync(command.ResourceId))
            {
                throw new ResourceAlreadyExistsException(command.ResourceId);
            }

            var resource = Resource.Create(command.ResourceId, command.Tags);
            await _resourcesRepository.AddAsync(resource);
        }
    }
}
