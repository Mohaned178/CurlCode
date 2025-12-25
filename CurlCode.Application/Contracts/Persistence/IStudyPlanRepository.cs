using System.Collections.Generic;
using CurlCode.Domain.Entities.StudyPlans;

namespace CurlCode.Application.Contracts.Persistence;

public interface IStudyPlanRepository : IGenericRepository<StudyPlan>
{
    Task<List<StudyPlan>> GetActiveStudyPlansAsync();
    Task<List<StudyPlan>> GetUserCustomStudyPlansAsync(string userId);
    Task<StudyPlan?> GetByIdWithItemsAsync(int id);
    Task<StudyPlan> CreateCustomStudyPlanAsync(StudyPlan studyPlan, List<StudyPlanItem> items);
    Task AddStudyPlanItemAsync(StudyPlanItem item);
    Task DeleteStudyPlanItemAsync(StudyPlanItem item);
    Task DeleteStudyPlanItemProgressesAsync(int studyPlanItemId);
    Task<StudyPlanProgress?> GetUserProgressAsync(int studyPlanId, string userId);
    Task<StudyPlanProgress> StartStudyPlanAsync(int studyPlanId, string userId);
    Task<bool> MarkProblemCompletedAsync(int studyPlanId, int problemId, string userId);
    Task<List<StudyPlanProgress>> GetUserStudyPlansAsync(string userId);
}
