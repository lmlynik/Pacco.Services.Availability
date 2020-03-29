using System;
namespace Pacco.Services.Availability.Core.Exceptions
{
    public abstract class DomainException: Exception
    {
        public abstract string Code { get; }
        protected DomainException(string message): base(message)
        {
        }
    }

    public class InvalidAggregateIdException : DomainException
    {
        public Guid Id { get; }

        public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id {id}")
        {
            Id = id;
        }

        public override string Code => nameof(InvalidAggregateIdException);
    }

    public class MissingResourceTagsException : DomainException
    {
        public MissingResourceTagsException() : base("Resource tags are missing")
        {
        }

        public override string Code => nameof(MissingResourceTagsException);
    }

    public class InvalidResourceTagsException : DomainException
    {
        public InvalidResourceTagsException() : base("Resource tags are invalid")
        {
        }

        public override string Code => nameof(InvalidResourceTagsException);
    }
}
