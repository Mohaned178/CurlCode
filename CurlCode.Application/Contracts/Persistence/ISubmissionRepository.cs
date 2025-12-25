using CurlCode.Application.Common.Models;
using CurlCode.Domain.Entities.Submissions;

namespace CurlCode.Application.Contracts.Persistence;

public interface ISubmissionRepository : IGenericRepository<Submission>
{
    Task<IEnumerable<Submission>> GetByUserIdAsync(string userId);
    Task<IEnumerable<Submission>> GetByProblemIdAsync(int problemId);
    Task<Submission?> GetByIdWithDetailsAsync(int id);
}






