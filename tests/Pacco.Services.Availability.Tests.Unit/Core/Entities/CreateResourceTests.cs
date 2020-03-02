using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Events;
using Pacco.Services.Availability.Core.Exceptions;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Core.Entities
{
    public class CreateResourceTests
    {
        Resource act(AggregateId id, IEnumerable<string> tags)
            => Resource.Create(id, tags);

        [Fact]
        public void given_valid_id_and_tags_resource_should_be_created()
        {
            // Arrange
            var id = new AggregateId();
            var tags = new []{"tag"};
            
            //Act
            var resource = act(id, tags);
            
            //Assert
            resource.ShouldNotBeNull();
            resource.Id.ShouldBe(id);
            resource.Tags.ShouldBe(tags);
        }

        [Fact]
        public void given_valid_id_and_tags_resource_should_create_domain_event_ResourceCreated()
        {
            // Arrange
            var id = new AggregateId();
            var tags = new []{"tag"};
            
            //Act
            var resource = act(id, tags);
            
            resource.Events.Count().ShouldBe(1);

            var domainEvent = resource.Events.First();
            domainEvent.ShouldBeOfType<ResourceCreated>();
        }

        [Fact]
        public void given_empty_tags_resource_should_throw_MissingResourceTagsException()
        {
            var id = new AggregateId();
            var tags = Enumerable.Empty<string>();

            var exception = Record.Exception(() => act(id, tags));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<MissingResourceTagsException>();
        }
    }
}