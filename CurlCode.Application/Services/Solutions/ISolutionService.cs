using CurlCode.Application.Common.Models;
using CurlCode.Application.DTOs.Solutions;

namespace CurlCode.Application.Services.Solutions;

public interface ISolutionService
{
    Task<PagedResult<SolutionDto>> GetSolutionsAsync(FilterParams filterParams, string? currentUserId);
    Task<SolutionDto?> GetSolutionByIdAsync(int id, string? currentUserId);
    Task<IEnumerable<SolutionDto>> GetSolutionsByProblemIdAsync(int problemId, string? currentUserId);
    Task<SolutionDto> CreateSolutionAsync(CreateSolutionDto dto, string userId);
    Task<SolutionDto> UpdateSolutionAsync(int id, CreateSolutionDto dto, string userId);
    Task DeleteSolutionAsync(int id, string userId);
    Task ToggleLikeAsync(int solutionId, string userId);
    Task ToggleDislikeAsync(int solutionId, string userId);
    Task<bool> IsDislikedByUserAsync(int solutionId, string userId);
    Task AddCommentAsync(int solutionId, string content, string userId);
    Task DeleteCommentAsync(int commentId, string userId);
    Task<IEnumerable<DTOs.Community.CommentDto>> GetCommentsAsync(int solutionId);
    Task<List<SolutionDto>> GetMostLikedSolutionsAsync(int count = 1);
}




