using System.Threading.Tasks;
using Pacco.Services.Availability.Core.Entites;

namespace Pacco.Services.Availability.Core.Repositories
{
    public interface IResourceRepository
    {
        Task<Resource> GetAsync(AggregateId id);
        Task<bool> ExistsAsync(AggregateId id);
        Task AddAsync(Resource resource);
        Task UpdateAsync(Resource resource);
        Task DeleteAsync(AggregateId id);
    }
}