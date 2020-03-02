using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Newtonsoft.Json;
using Pacco.Services.Availability.Api;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Tests.Shared.Factories;
using Pacco.Services.Availability.Tests.Shared.Fixtures;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.EndToEnd.Http
{
    public class AddResourceTests : IClassFixture<PaccoApplicationFactory<Program>>, IDisposable
    {
        Task<HttpResponseMessage> act(AddResource command)
            => _client.PostAsync("resources", GetStringContent(command));

        [Fact]
        public async Task given_valid_command_endpoint_should_return_201_http_response_code()
        {
            var command = new AddResource(Guid.NewGuid(), new[] {"tag"});

            var response = await act(command);
            
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
        }

        [Fact]
        public async Task given_valid_command_endpoint_should_return_valid_location_header()
        {
            var command = new AddResource(Guid.NewGuid(), new[] {"tag"});

            var response = await act(command);

            var locationHeader = response.Headers
                .FirstOrDefault(h => h.Key is "Location").Value.First();

            locationHeader.ShouldNotBeNull();
            locationHeader.ShouldBe($"resources/{command.ResourceId}");
        }

        [Fact]
        public async Task given_valid_command_resource_should_be_persisted_to_database()
        {
            var command = new AddResource(Guid.NewGuid(), new[] {"tag"});

            await act(command);

            var document = await _mongoDbFixture.GetAsync(command.ResourceId);
            
            document.ShouldNotBeNull();
            document.Id.ShouldBe(command.ResourceId);
            document.Tags.ShouldBe(command.Tags);
        }

        #region ARRANGE

        private readonly HttpClient _client;
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;

        public AddResourceTests(PaccoApplicationFactory<Program> factory)
        {
            factory.Server.AllowSynchronousIO = true;
            _client = factory.CreateClient();
            _mongoDbFixture = new MongoDbFixture<ResourceDocument, Guid>("resources");
        }
        
        private static StringContent GetStringContent(object @object)
            => new StringContent(JsonConvert.SerializeObject(@object), Encoding.UTF8, "application/json");

        public void Dispose()
        {
            _client?.Dispose();
            _mongoDbFixture?.Dispose();
        }
        
        #endregion
    }
}