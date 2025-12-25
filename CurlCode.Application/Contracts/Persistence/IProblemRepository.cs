using CurlCode.Application.Common.Models;
using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Application.Contracts.Persistence;

public interface IProblemRepository : IGenericRepository<Problem>
{
    Task<Problem?> GetByIdWithDetailsAsync(int id);
    Task<PagedResult<Problem>> GetPagedAsync(FilterParams filterParams);
    Task<IEnumerable<Problem>> GetByTopicIdAsync(int topicId);
    // Like/Dislike operations
    Task ToggleLikeAsync(int problemId, string userId);
    Task ToggleDislikeAsync(int problemId, string userId);
    Task<bool> IsLikedByUserAsync(int problemId, string userId);
    Task<bool> IsDislikedByUserAsync(int problemId, string userId);
    
    // Comment operations
    Task AddCommentAsync(int problemId, string userId, string content);
    Task DeleteCommentAsync(int commentId, string userId);
    Task<IEnumerable<Domain.Entities.Community.ProblemComment>> GetCommentsAsync(int problemId);

    Task<Problem?> GetRandomAsync();
    Task<List<Problem>> GetMostLikedProblemsAsync(int count);
}




