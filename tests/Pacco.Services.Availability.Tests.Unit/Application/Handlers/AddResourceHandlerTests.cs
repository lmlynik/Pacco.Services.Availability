using System;
using System.Threading.Tasks;
using NSubstitute;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Commands.Handlers;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Application.Handlers
{
    public class AddResourceHandlerTests
    {
        Task act(AddResource command)
            => _handler.HandleAsync(command);

        [Fact]
        public async Task given_already_existing_resource_handler_should_throw_ResourceAlreadyExistsException()
        {
            //Arrange
            var command = new AddResource(Guid.NewGuid(), null);
            _repository.ExistsAsync(command.ResourceId).Returns(true);
            
            //Act
            var exception = await Record.ExceptionAsync(async() => await act(command));
            
            //Assert
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceAlreadyExistsException>();
        }

        [Fact]
        public async Task given_valid_resource_handler_should_add_one_using_repository()
        {
            var command = new AddResource(Guid.NewGuid(), new[] {"tag"});

            await act(command);

            await _repository.Received(1).AddAsync(Arg.Is<Resource>(r =>
                r.Id == command.ResourceId));
        }

        #region ARRANGE

        private readonly AddResourceHandler _handler;
        private readonly IResourcesRepository _repository;
        private readonly IEventProcessor _processor;
        
        public AddResourceHandlerTests()
        {
            _repository = Substitute.For<IResourcesRepository>();
            _processor = Substitute.For<IEventProcessor>();
            _handler = new AddResourceHandler(_repository, _processor);   
        }

        #endregion
        
    }
}