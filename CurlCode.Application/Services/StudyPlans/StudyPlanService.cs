using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.StudyPlans;
using CurlCode.Domain.Entities.StudyPlans;

namespace CurlCode.Application.Services.StudyPlans;

public class StudyPlanService : IStudyPlanService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudyPlanService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<StudyPlanDto>> GetActiveStudyPlansAsync()
    {
        var studyPlans = await _unitOfWork.StudyPlans.GetActiveStudyPlansAsync();
        return _mapper.Map<List<StudyPlanDto>>(studyPlans);
    }

    public async Task<StudyPlanDto?> GetByIdAsync(int id)
    {
        var studyPlan = await _unitOfWork.StudyPlans.GetByIdWithItemsAsync(id);
        if (studyPlan == null)
            return null;

        return _mapper.Map<StudyPlanDto>(studyPlan);
    }

    public async Task<StudyPlanProgressDto?> GetUserProgressAsync(int studyPlanId, string userId)
    {
        var progress = await _unitOfWork.StudyPlans.GetUserProgressAsync(studyPlanId, userId);
        if (progress == null)
            return null;

        var dto = _mapper.Map<StudyPlanProgressDto>(progress);
        dto.TotalProblems = progress.StudyPlan.TotalProblems;
        dto.ProgressPercentage = progress.StudyPlan.TotalProblems > 0
            ? (double)progress.CompletedProblems / progress.StudyPlan.TotalProblems * 100
            : 0;
        return dto;
    }

    public async Task<StudyPlanProgressDto> StartStudyPlanAsync(int studyPlanId, string userId)
    {
        var progress = await _unitOfWork.StudyPlans.StartStudyPlanAsync(studyPlanId, userId);
        var dto = _mapper.Map<StudyPlanProgressDto>(progress);
        dto.TotalProblems = progress.StudyPlan.TotalProblems;
        dto.ProgressPercentage = 0;
        return dto;
    }

    public async Task<bool> MarkProblemCompletedAsync(int studyPlanId, int problemId, string userId)
    {
        return await _unitOfWork.StudyPlans.MarkProblemCompletedAsync(studyPlanId, problemId, userId);
    }

    public async Task<List<StudyPlanProgressDto>> GetUserStudyPlansAsync(string userId)
    {
        var progresses = await _unitOfWork.StudyPlans.GetUserStudyPlansAsync(userId);
        var dtos = _mapper.Map<List<StudyPlanProgressDto>>(progresses);

        foreach (var dto in dtos)
        {
            var progress = progresses.First(p => p.Id == dto.Id);
            dto.TotalProblems = progress.StudyPlan.TotalProblems;
            dto.ProgressPercentage = progress.StudyPlan.TotalProblems > 0
                ? (double)progress.CompletedProblems / progress.StudyPlan.TotalProblems * 100
                : 0;
        }

        return dtos;
    }

    public async Task<List<StudyPlanDto>> GetUserCustomStudyPlansAsync(string userId)
    {
        var studyPlans = await _unitOfWork.StudyPlans.GetUserCustomStudyPlansAsync(userId);
        return _mapper.Map<List<StudyPlanDto>>(studyPlans);
    }

    public async Task<StudyPlanDto> CreateCustomStudyPlanAsync(CreateStudyPlanDto dto, string userId)
    {
        var studyPlan = new StudyPlan
        {
            Title = dto.Title,
            Description = dto.Description,
            Difficulty = dto.Difficulty,
            DurationDays = dto.DurationDays,
            Deadline = dto.Deadline,
            UserId = userId,
            IsCustom = true,
            IsActive = true,
            TotalProblems = dto.Items.Count
        };

        var items = new List<StudyPlanItem>();
        foreach (var itemDto in dto.Items)
        {
            // Verify problem exists
            var problem = await _unitOfWork.Problems.GetByIdAsync(itemDto.ProblemId);
            if (problem == null)
                throw new NotFoundException("Problem", itemDto.ProblemId);

            items.Add(new StudyPlanItem
            {
                ProblemId = itemDto.ProblemId,
                DayNumber = itemDto.DayNumber,
                Order = itemDto.Order
            });
        }

        var createdPlan = await _unitOfWork.StudyPlans.CreateCustomStudyPlanAsync(studyPlan, items);
        return _mapper.Map<StudyPlanDto>(createdPlan);
    }

    public async Task<StudyPlanDto> CreateStudyPlanAsync(CreateStudyPlanDto dto)
    {
        var studyPlan = new StudyPlan
        {
            Title = dto.Title,
            Description = dto.Description,
            Difficulty = dto.Difficulty,
            DurationDays = dto.DurationDays,
            Deadline = dto.Deadline,
            IsCustom = false,
            IsActive = true,
            TotalProblems = dto.Items.Count
        };

        var items = new List<StudyPlanItem>();
        foreach (var itemDto in dto.Items)
        {
            // Verify problem exists
            var problem = await _unitOfWork.Problems.GetByIdAsync(itemDto.ProblemId);
            if (problem == null)
                throw new NotFoundException("Problem", itemDto.ProblemId);

            items.Add(new StudyPlanItem
            {
                ProblemId = itemDto.ProblemId,
                DayNumber = itemDto.DayNumber,
                Order = itemDto.Order
            });
        }

        var createdPlan = await _unitOfWork.StudyPlans.CreateCustomStudyPlanAsync(studyPlan, items);
        return _mapper.Map<StudyPlanDto>(createdPlan);
    }

    public async Task<StudyPlanDto> UpdateStudyPlanAsync(int id, CreateStudyPlanDto dto)
    {
        var studyPlan = await _unitOfWork.StudyPlans.GetByIdWithItemsAsync(id);
        if (studyPlan == null)
            throw new NotFoundException("StudyPlan", id);

        // Update study plan properties
        studyPlan.Title = dto.Title;
        studyPlan.Description = dto.Description;
        studyPlan.Difficulty = dto.Difficulty;
        studyPlan.DurationDays = dto.DurationDays;
        studyPlan.Deadline = dto.Deadline;
        studyPlan.TotalProblems = dto.Items.Count;
        studyPlan.UpdatedAt = DateTime.UtcNow;

        // Remove existing items
        var existingItems = studyPlan.Items.ToList();
        foreach (var item in existingItems)
        {
            await _unitOfWork.StudyPlans.DeleteStudyPlanItemAsync(item);
        }

        // Add new items
        foreach (var itemDto in dto.Items)
        {
            // Verify problem exists
            var problem = await _unitOfWork.Problems.GetByIdAsync(itemDto.ProblemId);
            if (problem == null)
                throw new NotFoundException("Problem", itemDto.ProblemId);

            var newItem = new StudyPlanItem
            {
                StudyPlanId = studyPlan.Id,
                ProblemId = itemDto.ProblemId,
                DayNumber = itemDto.DayNumber,
                Order = itemDto.Order
            };
            await _unitOfWork.StudyPlans.AddStudyPlanItemAsync(newItem);
        }

        await _unitOfWork.StudyPlans.UpdateAsync(studyPlan);
        await _unitOfWork.SaveChangesAsync();

        // Reload with items
        var updatedPlan = await _unitOfWork.StudyPlans.GetByIdWithItemsAsync(id);
        return _mapper.Map<StudyPlanDto>(updatedPlan!);
    }

    public async Task DeleteStudyPlanAsync(int id)
    {
        var studyPlan = await _unitOfWork.StudyPlans.GetByIdWithItemsAsync(id);
        if (studyPlan == null)
            throw new NotFoundException("StudyPlan", id);

        await _unitOfWork.StudyPlans.DeleteAsync(studyPlan);
        await _unitOfWork.SaveChangesAsync();
    }
}

