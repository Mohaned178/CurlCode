using CurlCode.Application.DTOs.StudyPlans;

namespace CurlCode.Application.Services.StudyPlans;

public interface IStudyPlanService
{
    Task<List<StudyPlanDto>> GetActiveStudyPlansAsync();
    Task<List<StudyPlanDto>> GetUserCustomStudyPlansAsync(string userId);
    Task<StudyPlanDto?> GetByIdAsync(int id);
    Task<StudyPlanDto> CreateCustomStudyPlanAsync(CreateStudyPlanDto dto, string userId);
    Task<StudyPlanDto> CreateStudyPlanAsync(CreateStudyPlanDto dto);
    Task<StudyPlanDto> UpdateStudyPlanAsync(int id, CreateStudyPlanDto dto);
    Task DeleteStudyPlanAsync(int id);
    Task<StudyPlanProgressDto?> GetUserProgressAsync(int studyPlanId, string userId);
    Task<StudyPlanProgressDto> StartStudyPlanAsync(int studyPlanId, string userId);
    Task<bool> MarkProblemCompletedAsync(int studyPlanId, int problemId, string userId);
    Task<List<StudyPlanProgressDto>> GetUserStudyPlansAsync(string userId);
}

