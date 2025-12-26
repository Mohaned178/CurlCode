using CurlCode.Application.Common.Models;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Enums;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using CurlCode.Domain.Entities.Community;
using System;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class ProblemRepository : GenericRepository<Problem>, IProblemRepository
{
    public ProblemRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Problem?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(p => p.ProblemTopics)
                .ThenInclude(pt => pt.Topic)
            .Include(p => p.TestCases)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<PagedResult<Problem>> GetPagedAsync(FilterParams filterParams)
    {
        var query = _dbSet
            .Include(p => p.ProblemTopics)
                .ThenInclude(pt => pt.Topic)
            .AsQueryable();


        if (!string.IsNullOrWhiteSpace(filterParams.SearchTerm))
        {
            query = query.Where(p => p.Title.Contains(filterParams.SearchTerm) ||
                                    p.Description.Contains(filterParams.SearchTerm));
        }

        if (filterParams.Difficulty.HasValue)
        {
            query = query.Where(p => p.Difficulty == filterParams.Difficulty.Value);
        }

        if (filterParams.TopicId.HasValue)
        {
            query = query.Where(p => p.ProblemTopics.Any(pt => pt.TopicId == filterParams.TopicId.Value));
        }


        query = filterParams.SortBy?.ToLower() switch
        {
            "title" => filterParams.SortOrder == "asc" 
                ? query.OrderBy(p => p.Title) 
                : query.OrderByDescending(p => p.Title),
            "difficulty" => filterParams.SortOrder == "asc"
                ? query.OrderBy(p => p.Difficulty)
                : query.OrderByDescending(p => p.Difficulty),
            "acceptancerate" => filterParams.SortOrder == "asc"
                ? query.OrderBy(p => p.AcceptanceRate)
                : query.OrderByDescending(p => p.AcceptanceRate),
            _ => filterParams.SortOrder == "asc"
                ? query.OrderBy(p => p.CreatedAt)
                : query.OrderByDescending(p => p.CreatedAt)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
            .Take(filterParams.PageSize)
            .ToListAsync();

        return new PagedResult<Problem>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filterParams.PageNumber,
            PageSize = filterParams.PageSize
        };
    }

    public async Task<IEnumerable<Problem>> GetByTopicIdAsync(int topicId)
    {
        return await _dbSet
            .Include(p => p.ProblemTopics)
                .ThenInclude(pt => pt.Topic)
            .Where(p => p.ProblemTopics.Any(pt => pt.TopicId == topicId))
            .ToListAsync();
    }

    public async Task ToggleLikeAsync(int problemId, string userId)
    {
        var problem = await _dbSet
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .FirstOrDefaultAsync(p => p.Id == problemId);
        
        if (problem == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Problem), problemId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        var existingLike = problem.Likes.FirstOrDefault(l => l.UserId == userId);
        var existingDislike = problem.Dislikes.FirstOrDefault(d => d.UserId == userId);
        
        if (existingLike != null)
        {

            problem.Likes.Remove(existingLike);
            problem.LikesCount = Math.Max(0, problem.LikesCount - 1);
        }
        else if (existingDislike != null)
        {

            problem.Dislikes.Remove(existingDislike);
            problem.DislikesCount = Math.Max(0, problem.DislikesCount - 1);
            problem.Likes.Add(new ProblemLike { ProblemId = problemId, UserId = userId });
            problem.LikesCount++;
        }
        else
        {

            problem.Likes.Add(new ProblemLike { ProblemId = problemId, UserId = userId });
            problem.LikesCount++;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task ToggleDislikeAsync(int problemId, string userId)
    {
        var problem = await _dbSet
            .Include(p => p.Likes)
            .Include(p => p.Dislikes)
            .FirstOrDefaultAsync(p => p.Id == problemId);
        
        if (problem == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Problem), problemId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        var existingLike = problem.Likes.FirstOrDefault(l => l.UserId == userId);
        var existingDislike = problem.Dislikes.FirstOrDefault(d => d.UserId == userId);
        
        if (existingDislike != null)
        {

            problem.Dislikes.Remove(existingDislike);
            problem.DislikesCount = Math.Max(0, problem.DislikesCount - 1);
        }
        else if (existingLike != null)
        {

            problem.Likes.Remove(existingLike);
            problem.LikesCount = Math.Max(0, problem.LikesCount - 1);
            problem.Dislikes.Add(new ProblemDislike { ProblemId = problemId, UserId = userId });
            problem.DislikesCount++;
        }
        else
        {

            problem.Dislikes.Add(new ProblemDislike { ProblemId = problemId, UserId = userId });
            problem.DislikesCount++;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsLikedByUserAsync(int problemId, string userId)
    {
        var problem = await _dbSet.Include(p => p.Likes).FirstOrDefaultAsync(p => p.Id == problemId);
        return problem != null && problem.Likes.Any(l => l.UserId == userId);
    }

    public async Task<bool> IsDislikedByUserAsync(int problemId, string userId)
    {
        var problem = await _dbSet.Include(p => p.Dislikes).FirstOrDefaultAsync(p => p.Id == problemId);
        return problem != null && problem.Dislikes.Any(d => d.UserId == userId);
    }

    public async Task AddCommentAsync(int problemId, string userId, string content)
    {
        var problem = await _dbSet.FirstOrDefaultAsync(p => p.Id == problemId);
        if (problem == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(Problem), problemId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        var comment = new ProblemComment 
        { 
            ProblemId = problemId, 
            UserId = userId, 
            Content = content 
        };
        
        _context.Set<ProblemComment>().Add(comment);
        problem.CommentsCount++;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int commentId, string userId)
    {
        var comment = await _context.Set<ProblemComment>()
            .Include(c => c.Problem)
            .FirstOrDefaultAsync(c => c.Id == commentId);
        
        if (comment == null) 
            throw new CurlCode.Application.Common.Exceptions.NotFoundException(nameof(ProblemComment), commentId);
        
        if (string.IsNullOrWhiteSpace(userId))
            throw new CurlCode.Application.Common.Exceptions.ValidationException("UserId cannot be empty");
        
        if (comment.UserId != userId) 
            throw new UnauthorizedAccessException("You can only delete your own comments");
        
        comment.Problem.CommentsCount = Math.Max(0, comment.Problem.CommentsCount - 1);
        _context.Set<ProblemComment>().Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProblemComment>> GetCommentsAsync(int problemId)
    {
        return await _context.Set<ProblemComment>()
            .Include(c => c.User)
            .Where(c => c.ProblemId == problemId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Problem?> GetRandomAsync()
    {
        return await _dbSet
            .OrderBy(_ => Guid.NewGuid())
            .FirstOrDefaultAsync();
    }

    public async Task<List<Problem>> GetMostLikedProblemsAsync(int count)
    {
        return await _dbSet
            .Include(p => p.ProblemTopics).ThenInclude(pt => pt.Topic)
            .OrderByDescending(p => p.LikesCount)
            .ThenByDescending(p => p.TotalSubmissions)
            .Take(count)
            .ToListAsync();
    }
}




