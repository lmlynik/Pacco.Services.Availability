using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using Pacco.Services.Availability.Api;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Queries;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;
using Pacco.Services.Availability.Tests.Shared.Factories;
using Pacco.Services.Availability.Tests.Shared.Fixtures;
using Shouldly;
using Xunit;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Pacco.Services.Availability.Tests.EndToEnd.Http
{
    public class GetResourceTests : IClassFixture<PaccoApplicationFactory<Program>>, IDisposable
    {
        Task<HttpResponseMessage> act(Guid resourceId)
            => _client.GetAsync($"resources/{resourceId}");

        [Fact]
        public async Task given_invalid_resourceId_http_404_status_code_should_be_returned()
        {
            var resourceId = Guid.NewGuid();

            var response = await act(resourceId);
            
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task given_valid_resourceId_http_200_status_code_should_be_returned()
        {
            //ARRANGE 
            var document = new ResourceDocument
            {
                Id = Guid.NewGuid(),
                Tags = new [] {"tag"}
            };

            await _mongoDbFixture.InsertAsync(document);

            var response = await act(document.Id);

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<ResourceDto>(json);

            //ASSERT 
            dto.Id.ShouldBe(document.Id);
            dto.Tags.ShouldBe(document.Tags);
        }
        
        #region ARRANGE

        private readonly HttpClient _client;
        private readonly MongoDbFixture<ResourceDocument, Guid> _mongoDbFixture;

        public GetResourceTests(PaccoApplicationFactory<Program> factory)
        {
            factory.Server.AllowSynchronousIO = true;
            _client = factory.CreateClient();
            _mongoDbFixture = new MongoDbFixture<ResourceDocument, Guid>("resources");
        }

        public void Dispose()
        {
            _client?.Dispose();
            _mongoDbFixture?.Dispose();
        }
        
        #endregion
    }
}