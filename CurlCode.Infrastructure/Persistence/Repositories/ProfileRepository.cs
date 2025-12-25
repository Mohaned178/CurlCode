using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class ProfileRepository : GenericRepository<UserProfile>, IProfileRepository
{
    public ProfileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId)
    {
        return await _dbSet
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<UserProfile?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.User.UserName == username);
    }

    public async Task<int> GetUserRankAsync(int totalScore)
    {
        return await _dbSet
            .CountAsync(p => p.TotalScore > totalScore) + 1;
    }
}






