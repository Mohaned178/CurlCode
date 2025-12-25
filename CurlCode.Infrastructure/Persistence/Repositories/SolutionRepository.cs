using CurlCode.Application.Common.Models;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Community;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class SolutionRepository : GenericRepository<Solution>, ISolutionRepository
{
    public SolutionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Solution?> GetByIdWithDetailsAsync(int id, string? currentUserId = null)
    {
        var solution = await _dbSet
            .Include(s => s.Problem)
            .Include(s => s.User)
            .Include(s => s.Likes)
            .Include(s => s.Comments)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(s => s.Id == id);

        return solution;
    }

    public async Task<PagedResult<Solution>> GetPagedAsync(FilterParams filterParams, string? currentUserId = null)
    {
        var query = _dbSet
            .Include(s => s.Problem)
            .Include(s => s.User)
            .Include(s => s.Likes)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
        {
            query = query.Where(s => s.Title.Contains(filterParams.SearchTerm) ||
                                    s.Content.Contains(filterParams.SearchTerm));
        }

        // Apply sorting
        query = filterParams.SortBy?.ToLower() switch
        {
            "mostliked" => query.OrderByDescending(s => s.LikesCount),
            "newest" => query.OrderByDescending(s => s.CreatedAt),
            _ => query.OrderByDescending(s => s.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
            .Take(filterParams.PageSize)
            .ToListAsync();

        return new PagedResult<Solution>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filterParams.PageNumber,
            PageSize = filterParams.PageSize
        };
    }

    public async Task<IEnumerable<Solution>> GetByProblemIdAsync(int problemId, string? currentUserId = null)
    {
        return await _dbSet
            .Include(s => s.User)
            .Include(s => s.Likes)
            .Where(s => s.ProblemId == problemId)
            .OrderByDescending(s => s.LikesCount)
            .ThenByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsLikedByUserAsync(int solutionId, string userId)
    {
        return await _dbSet
            .AnyAsync(s => s.Id == solutionId && s.Likes.Any(l => l.UserId == userId));
    }

    public async Task ToggleLikeAsync(int solutionId, string userId)
    {
        var solution = await _dbSet
            .Include(s => s.Likes)
            .Include(s => s.Dislikes)
            .FirstOrDefaultAsync(s => s.Id == solutionId);
        
        if (solution == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Solution), solutionId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        var existingLike = solution.Likes.FirstOrDefault(l => l.UserId == userId);
        var existingDislike = solution.Dislikes.FirstOrDefault(d => d.UserId == userId);
        
        if (existingLike != null)
        {
            // Remove like
            solution.Likes.Remove(existingLike);
            solution.LikesCount = Math.Max(0, solution.LikesCount - 1);
        }
        else if (existingDislike != null)
        {
            // Switch from dislike to like
            solution.Dislikes.Remove(existingDislike);
            solution.DislikesCount = Math.Max(0, solution.DislikesCount - 1);
            solution.Likes.Add(new SolutionLike { SolutionId = solutionId, UserId = userId });
            solution.LikesCount++;
        }
        else
        {
            // Add like
            solution.Likes.Add(new SolutionLike { SolutionId = solutionId, UserId = userId });
            solution.LikesCount++;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task ToggleDislikeAsync(int solutionId, string userId)
    {
        var solution = await _dbSet
            .Include(s => s.Likes)
            .Include(s => s.Dislikes)
            .FirstOrDefaultAsync(s => s.Id == solutionId);
        
        if (solution == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Solution), solutionId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        var existingLike = solution.Likes.FirstOrDefault(l => l.UserId == userId);
        var existingDislike = solution.Dislikes.FirstOrDefault(d => d.UserId == userId);
        
        if (existingDislike != null)
        {
            // Remove dislike
            solution.Dislikes.Remove(existingDislike);
            solution.DislikesCount = Math.Max(0, solution.DislikesCount - 1);
        }
        else if (existingLike != null)
        {
            // Switch from like to dislike
            solution.Likes.Remove(existingLike);
            solution.LikesCount = Math.Max(0, solution.LikesCount - 1);
            solution.Dislikes.Add(new SolutionDislike { SolutionId = solutionId, UserId = userId });
            solution.DislikesCount++;
        }
        else
        {
            // Add dislike
            solution.Dislikes.Add(new SolutionDislike { SolutionId = solutionId, UserId = userId });
            solution.DislikesCount++;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsDislikedByUserAsync(int solutionId, string userId)
    {
        return await _dbSet
            .AnyAsync(s => s.Id == solutionId && s.Dislikes.Any(d => d.UserId == userId));
    }

    public async Task AddCommentAsync(int solutionId, string userId, string content)
    {
        var solution = await _dbSet.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == solutionId);
        if (solution == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Solution), solutionId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        solution.Comments.Add(new Comment { SolutionId = solutionId, UserId = userId, Content = content });
        solution.CommentsCount++;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId, string userId)
    {
        var comment = await _context.Set<Comment>()
            .Include(c => c.Solution)
            .FirstOrDefaultAsync(c => c.Id == commentId);
        
        if (comment == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Comment), commentId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        if (comment.UserId != userId) 
            throw new UnauthorizedAccessException("You can only delete your own comments");
        
        comment.Solution.CommentsCount = Math.Max(0, comment.Solution.CommentsCount - 1);
        _context.Set<Comment>().Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsAsync(int solutionId)
    {
        return await _context.Set<Comment>()
            .Include(c => c.User)
            .Where(c => c.SolutionId == solutionId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Solution>> GetMostLikedSolutionsAsync(int count)
    {
        return await _dbSet
            .Include(s => s.Problem)
            .Include(s => s.User)
            .OrderByDescending(s => s.LikesCount)
            .ThenByDescending(s => s.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}




