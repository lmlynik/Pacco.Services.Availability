using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pacco.Services.Availability.Api;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Tests.Shared.Factories;
using Pacco.Services.Availability.Tests.Shared.Fixtures;
using RabbitMQ.Client.Impl;
using Shouldly;
using Xunit;

namespace Pacco.Services.Availability.Tests.EndToEnd.Http
{
    public class ReserveResourceTests : IClassFixture<PaccoApplicationFactory<Program>>, IDisposable
    {
        public Task<HttpResponseMessage> act(ReserveResource command)
            => _client.PostAsync($"resources/{command.ResourceId}/reservations/{command.DateTime:yyyy-MM-dd}",
                GetStringContent(command));

        [Fact]
        public async Task given_reservation_for_non_existing_resource_endpoint_should_return_400_http_status_code()
        {
            var command = new ReserveResource(Guid.NewGuid(), Guid.Empty, new DateTime(), 1);

            var response = await act(command);
            
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var json = await response.Content.ReadAsStringAsync();
            var errorCode = JsonConvert.DeserializeObject<ErrorCode>(json);
            
            errorCode.ShouldNotBeNull();
            errorCode.Code.ShouldBe("resource_not_found");
        }

        #region ARRANGE
        
        private const string ValidCustomerId = "e953c6d2-ca3d-4963-9c2f-269ca89dccc8";
        private const string InvalidCustomerId = "f453c6d2-ca3d-4963-9c2f-269ca89dccc8";

        private readonly HttpClient _client;
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;

        public ReserveResourceTests(PaccoApplicationFactory<Program> factory)
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

        private class ErrorCode
        {
            public string Code { get; set; }
        }
        
        #endregion
    }
}