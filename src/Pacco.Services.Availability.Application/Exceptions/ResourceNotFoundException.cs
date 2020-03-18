using System;
using System.Runtime.Serialization;

namespace Pacco.Services.Availability.Application.Exceptions
{
    [Serializable]
    internal class ResourceNotFoundException : AppException
    {
        private Guid resourceId;

        public ResourceNotFoundException(Guid resourceId):base($"Resource not found {resourceId}")
        {
            this.resourceId = resourceId;
        }

        public override string Code => nameof(ResourceNotFoundException);
    }
}