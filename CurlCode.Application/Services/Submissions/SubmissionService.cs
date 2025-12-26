using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Submissions;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Entities.Submissions;
using CurlCode.Domain.Enums;

namespace CurlCode.Application.Services.Submissions;

public class SubmissionService : ISubmissionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SubmissionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SubmissionResultDto> SubmitCodeAsync(SubmitCodeRequest request, string userId)
    {
        var problem = await _unitOfWork.Problems.GetByIdWithDetailsAsync(request.ProblemId);
        if (problem == null)
        {
            throw new NotFoundException(nameof(Problem), request.ProblemId);
        }

        var submission = new Submission
        {
            ProblemId = request.ProblemId,
            UserId = userId,
            Code = request.Code,
            Language = request.Language,
            Status = SubmissionStatus.Pending
        };

        await _unitOfWork.Submissions.AddAsync(submission);
        
        problem.TotalSubmissions++;
        
        await _unitOfWork.SaveChangesAsync();

        await ProcessSubmissionAsync(submission, problem);

        await _unitOfWork.Submissions.UpdateAsync(submission);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SubmissionResultDto>(submission);
    }

    private async Task ProcessSubmissionAsync(Submission submission, Problem problem)
    {
        await Task.Delay(100);
        
        var testCases = problem.TestCases.Where(tc => !tc.IsHidden).ToList();
        submission.TotalTestCases = testCases.Count;
        submission.TestCasesPassed = testCases.Count;
        submission.Status = SubmissionStatus.Accepted;
        submission.ExecutionTimeMs = 50;
        submission.MemoryUsedMb = 10;
        
        if (submission.Status == SubmissionStatus.Accepted)
        {
            problem.AcceptedSubmissions++;
        }
    }

    public async Task<IEnumerable<SubmissionResultDto>> GetMySubmissionsAsync(string userId)
    {
        var submissions = await _unitOfWork.Submissions.GetByUserIdAsync(userId);
        return _mapper.Map<List<SubmissionResultDto>>(submissions);
    }

    public async Task<SubmissionResultDto?> GetSubmissionByIdAsync(int id, string userId)
    {
        var submission = await _unitOfWork.Submissions.GetByIdWithDetailsAsync(id);
        if (submission == null)
        {
            return null;
        }

        if (submission.UserId != userId)
        {
            throw new Common.Exceptions.ValidationException("You can only view your own submissions.");
        }

        return _mapper.Map<SubmissionResultDto>(submission);
    }
}






