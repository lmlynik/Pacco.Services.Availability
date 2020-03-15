﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pacco.Services.Availability.Application.Exceptions
{
    public abstract class AppException: Exception
    {
        public abstract string Code { get; }

        protected AppException(string message) : base(message)
        {

        }
    }

    public class ResourceAlreadyExistsException : AppException
    {
        public override string Code => nameof(ResourceAlreadyExistsException);

        public Guid ResourceId { get; }

        public ResourceAlreadyExistsException(Guid resourceId):base($"Resource with id {resourceId} already exists")
        {
            this.ResourceId = resourceId;
        }
    }
}
