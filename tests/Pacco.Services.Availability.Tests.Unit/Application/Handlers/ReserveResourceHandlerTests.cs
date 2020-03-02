using System;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Commands.Handlers;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Application.Services.Clients;
using Pacco.Services.Availability.Core.Entities;
using Pacco.Services.Availability.Core.Repositories;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.Unit.Application.Handlers
{
    public class ReserveResourceHandlerTests
    {
        Task act(ReserveResource command)
            => _handler.HandleAsync(command);

        [Fact]
        public async Task given_reservation_for_non_existing_resource_handler_should_throw_ResourceNotFoundException()
        {
            var command = new ReserveResource(Guid.NewGuid(), Guid.Empty, new DateTime(), 1 );

            var exception = await Record.ExceptionAsync(async () => await act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ResourceNotFoundException>();
        }
        
        [Fact]
        public async Task given_reservation_for_non_existing_customer_handler_should_throw_CustomerNotFoundException()
        {
            var command = new ReserveResource(Guid.NewGuid(), Guid.Empty, new DateTime(), 1 );
            _repository.GetAsync(command.ResourceId).Returns(getResource());

            var exception = await Record.ExceptionAsync(async () => await act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<CustomerNotFoundException>();
        }
        

        [Fact]
        public async Task given_reservation_for_invalid_customer_handler_should_throw_InvalidCustomerStateException()
        {
            var command = new ReserveResource(Guid.NewGuid(), Guid.Empty, new DateTime(), 1 );
            
            _repository.GetAsync(command.ResourceId).Returns(getResource());
            _client.GetStateAsync(command.CustomerId).Returns(getCustomerState(isValid: false));

            var exception = await Record.ExceptionAsync(async () => await act(command));
            
            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<InvalidCustomerStateException>();
        }
        
        [Fact]
        public async Task given_reservation_for_valid_customer_handler_should_update_resource_with_new_reservation()
        {
            var command = new ReserveResource(Guid.NewGuid(), Guid.Empty, new DateTime(), 1 );
            var resource = getResource();
            
            _repository.GetAsync(command.ResourceId).Returns(resource);
            _client.GetStateAsync(command.CustomerId).Returns(getCustomerState(isValid: true));

            await act(command);
            await _repository.Received().UpdateAsync(Arg.Is(resource));
        }
        
        
        #region ARRANGE

        private readonly ReserveResourceHandler _handler;
        private readonly ICustomersServiceClient _client;
        private readonly IResourcesRepository _repository;
        private readonly IEventProcessor _processor;
        
        public ReserveResourceHandlerTests()
        {
            _client = Substitute.For<ICustomersServiceClient>();
            _repository = Substitute.For<IResourcesRepository>();
            _processor = Substitute.For<IEventProcessor>();
            _handler = new ReserveResourceHandler(_client,_repository, _processor);   
        }

        private static Resource getResource()
            => new Resource(Guid.NewGuid(), new [] {"tag"});
        
        private static CustomerStateDto getCustomerState(bool isValid)
            => new CustomerStateDto{ State = isValid? "valid" : "invalid"};

        #endregion
    }
}