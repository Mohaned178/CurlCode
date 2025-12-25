using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Topic?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
    }
}






