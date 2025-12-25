using CurlCode.Application.Contracts.Persistence;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Entities.StudyPlans;
using CurlCode.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CurlCode.Infrastructure.Persistence.Repositories;

public class StudyPlanRepository : GenericRepository<StudyPlan>, IStudyPlanRepository
{
    public StudyPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<StudyPlan>> GetActiveStudyPlansAsync()
    {
        return await _dbSet
            .Where(sp => sp.IsActive && !sp.IsCustom)
            .Include(sp => sp.Items)
                .ThenInclude(spi => spi.Problem)
            .OrderBy(sp => sp.Difficulty)
            .ThenBy(sp => sp.DurationDays)
            .ToListAsync();
    }

    public async Task<List<StudyPlan>> GetUserCustomStudyPlansAsync(string userId)
    {
        return await _dbSet
            .Where(sp => sp.IsCustom && sp.UserId == userId)
            .Include(sp => sp.Items)
                .ThenInclude(spi => spi.Problem)
            .OrderByDescending(sp => sp.CreatedAt)
            .ToListAsync();
    }

    public async Task<StudyPlan> CreateCustomStudyPlanAsync(StudyPlan studyPlan, List<StudyPlanItem> items)
    {
        studyPlan.IsCustom = true;
        studyPlan.TotalProblems = items.Count;
        studyPlan.IsActive = true;

        await _dbSet.AddAsync(studyPlan);
        await _context.SaveChangesAsync(); // Save to get ID

        foreach (var item in items)
        {
            item.StudyPlanId = studyPlan.Id;
            await _context.StudyPlanItems.AddAsync(item);
        }

        await _context.SaveChangesAsync();
        return studyPlan;
    }

    public async Task<StudyPlan?> GetByIdWithItemsAsync(int id)
    {
        return await _dbSet
            .Include(sp => sp.Items)
                .ThenInclude(spi => spi.Problem)
            .FirstOrDefaultAsync(sp => sp.Id == id);
    }

    public async Task<StudyPlanProgress?> GetUserProgressAsync(int studyPlanId, string userId)
    {
        return await _context.StudyPlanProgresses
            .Include(spp => spp.StudyPlan)
            .Include(spp => spp.ItemProgresses)
                .ThenInclude(spp => spp.StudyPlanItem)
                    .ThenInclude(spi => spi.Problem)
            .FirstOrDefaultAsync(spp => spp.StudyPlanId == studyPlanId && spp.UserId == userId);
    }

    public async Task<StudyPlanProgress> StartStudyPlanAsync(int studyPlanId, string userId)
    {
        var existingProgress = await GetUserProgressAsync(studyPlanId, userId);
        if (existingProgress != null)
            return existingProgress;

        var studyPlan = await GetByIdWithItemsAsync(studyPlanId);
        if (studyPlan == null)
            throw new Exception($"Study plan {studyPlanId} not found");

        var progress = new StudyPlanProgress
        {
            StudyPlanId = studyPlanId,
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            CompletedProblems = 0
        };

        await _context.StudyPlanProgresses.AddAsync(progress);
        await _context.SaveChangesAsync(); // Save to get the ID

        // Create item progresses for all items
        foreach (var item in studyPlan.Items)
        {
            var itemProgress = new StudyPlanItemProgress
            {
                StudyPlanProgressId = progress.Id,
                StudyPlanItemId = item.Id,
                IsCompleted = false
            };
            await _context.StudyPlanItemProgresses.AddAsync(itemProgress);
        }

        await _context.SaveChangesAsync();
        return progress;
    }

    public async Task<bool> MarkProblemCompletedAsync(int studyPlanId, int problemId, string userId)
    {
        var progress = await GetUserProgressAsync(studyPlanId, userId);
        if (progress == null)
            return false;

        // Get the study plan item for this problem
        var studyPlan = await GetByIdWithItemsAsync(studyPlanId);
        if (studyPlan == null)
            return false;

        var item = studyPlan.Items.FirstOrDefault(spi => spi.ProblemId == problemId);
        if (item == null)
            return false;

        var itemProgress = progress.ItemProgresses.FirstOrDefault(spp => spp.StudyPlanItemId == item.Id);
        if (itemProgress == null || itemProgress.IsCompleted)
            return false;

        itemProgress.IsCompleted = true;
        itemProgress.CompletedAt = DateTime.UtcNow;
        progress.CompletedProblems++;

        // Check if all problems are completed
        if (progress.CompletedProblems >= studyPlan.TotalProblems)
        {
            progress.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<StudyPlanProgress>> GetUserStudyPlansAsync(string userId)
    {
        return await _context.StudyPlanProgresses
            .Include(spp => spp.StudyPlan)
            .Include(spp => spp.ItemProgresses)
                .ThenInclude(spp => spp.StudyPlanItem)
                    .ThenInclude(spi => spi.Problem)
            .Where(spp => spp.UserId == userId)
            .OrderByDescending(spp => spp.StartedAt)
            .ToListAsync();
    }

    public async Task AddStudyPlanItemAsync(StudyPlanItem item)
    {
        await _context.StudyPlanItems.AddAsync(item);
    }

    public async Task DeleteStudyPlanItemAsync(StudyPlanItem item)
    {
        // First delete associated item progresses
        await DeleteStudyPlanItemProgressesAsync(item.Id);
        
        _context.StudyPlanItems.Remove(item);
    }

    public async Task DeleteStudyPlanItemProgressesAsync(int studyPlanItemId)
    {
        var itemProgresses = await _context.StudyPlanItemProgresses
            .Where(spp => spp.StudyPlanItemId == studyPlanItemId)
            .ToListAsync();
        
        if (itemProgresses.Any())
        {
            _context.StudyPlanItemProgresses.RemoveRange(itemProgresses);
        }
    }
}

