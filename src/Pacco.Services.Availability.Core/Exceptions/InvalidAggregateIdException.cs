using System;
namespace Pacco.Services.Availability.Core.Exceptions
{
    public class InvalidAggregateIdException: DomainException
    {
        public Guid Id { get; }

        public InvalidAggregateIdException(Guid id): base($"Invalid aggregate id {id}")
        {
            Id = id;
        }

        public override string Code => nameof(InvalidAggregateIdException);
    }
}
