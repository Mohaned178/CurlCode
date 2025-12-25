using CurlCode.Application.DTOs.Submissions;

namespace CurlCode.Application.Services.Submissions;

public interface ISubmissionService
{
    Task<SubmissionResultDto> SubmitCodeAsync(SubmitCodeRequest request, string userId);
    Task<IEnumerable<SubmissionResultDto>> GetMySubmissionsAsync(string userId);
    Task<SubmissionResultDto?> GetSubmissionByIdAsync(int id, string userId);
}






