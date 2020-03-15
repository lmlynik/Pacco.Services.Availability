using System;
using System.Threading.Tasks;
using Convey.Persistence.MongoDB;
using MongoDB.Driver;
using Pacco.Services.Availability.Core.Entites;
using Pacco.Services.Availability.Core.Repositories;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Availability.Infrastructure.Mongo.Repositories
{
    internal sealed class ResourcesMongoRepository : IResourcesRepository
    {
        private readonly IMongoRepository<ResourceDocument, Guid> _repository;

        public ResourcesMongoRepository(IMongoRepository<ResourceDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(Resource resource) => _repository.AddAsync(resource.AsDocument());

        public Task DeleteAsync(AggregateId id) => _repository.DeleteAsync(id);

        public async Task<bool> ExistsAsync(AggregateId id) => await _repository.ExistsAsync(r => r.Id == id);

        public async Task<Resource> GetAsync(AggregateId id)
        {
            var doc = await _repository.GetAsync(r => r.Id == id);

            return doc?.AsEntity();
        }
               
        public Task UpdateAsync(Resource resource)
         => _repository.Collection.ReplaceOneAsync(
             r => r.Id == resource.Id && r.Version < resource.Version,
             resource.AsDocument());
    }
}
