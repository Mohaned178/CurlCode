using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Submissions;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class SubmissionRepository : GenericRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Submission>> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(s => s.Problem)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Submission>> GetByProblemIdAsync(int problemId)
    {
        return await _dbSet
            .Include(s => s.User)
            .Where(s => s.ProblemId == problemId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Submission?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Problem)
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}






