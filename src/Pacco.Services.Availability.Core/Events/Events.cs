using System;
using Pacco.Services.Availability.Core.Entites;

namespace Pacco.Services.Availability.Core.Events
{
    public interface IDomainEvent
    {
    }

    public class ResourceCreated: IDomainEvent
    {
        public Resource Resource { get; set; }

        public ResourceCreated(Resource resource)
        {
            Resource = resource;
        }
    }
}
