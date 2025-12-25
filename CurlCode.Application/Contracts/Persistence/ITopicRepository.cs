using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Application.Contracts.Persistence;

public interface ITopicRepository : IGenericRepository<Topic>
{
    Task<Topic?> GetByNameAsync(string name);
}






