using CurlCode.Application.Common.Models;
using CurlCode.Domain.Entities.Community;

namespace CurlCode.Application.Contracts.Persistence;

public interface ISolutionRepository : IGenericRepository<Solution>
{
    Task<Solution?> GetByIdWithDetailsAsync(int id, string? currentUserId = null);
    Task<PagedResult<Solution>> GetPagedAsync(FilterParams filterParams, string? currentUserId = null);
    Task<IEnumerable<Solution>> GetByProblemIdAsync(int problemId, string? currentUserId = null);
    Task<bool> IsLikedByUserAsync(int solutionId, string userId);
    Task<bool> IsDislikedByUserAsync(int solutionId, string userId);
    Task ToggleLikeAsync(int solutionId, string userId);
    Task ToggleDislikeAsync(int solutionId, string userId);
    Task AddCommentAsync(int solutionId, string userId, string content);
    Task DeleteCommentAsync(int commentId, string userId);
    Task<IEnumerable<Comment>> GetCommentsAsync(int solutionId);
    Task<List<Solution>> GetMostLikedSolutionsAsync(int count);
}




