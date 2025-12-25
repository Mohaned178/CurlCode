using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Application.Contracts.Persistence;

public interface IProfileRepository : IGenericRepository<UserProfile>
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task<UserProfile?> GetByUsernameAsync(string username);
    Task<int> GetUserRankAsync(int totalScore);
}






