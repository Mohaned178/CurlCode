using CurlCode.Application.DTOs.StudyPlans;
using FluentValidation;

namespace CurlCode.Application.Validators;

public class CreateStudyPlanDtoValidator : AbstractValidator<CreateStudyPlanDto>
{
    public CreateStudyPlanDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(3, 200).WithMessage("Title must be between 3 and 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level");

        RuleFor(x => x.DurationDays)
            .InclusiveBetween(1, 365).WithMessage("Duration must be between 1 and 365 days");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.UtcNow).When(x => x.Deadline.HasValue)
            .WithMessage("Deadline must be in the future");
            
        RuleForEach(x => x.Items).SetValidator(new StudyPlanItemCreateDtoValidator());
    }
}

public class StudyPlanItemCreateDtoValidator : AbstractValidator<StudyPlanItemCreateDto>
{
    public StudyPlanItemCreateDtoValidator()
    {
        RuleFor(x => x.ProblemId)
            .GreaterThan(0).WithMessage("Invalid Problem ID");

        RuleFor(x => x.DayNumber)
            .GreaterThan(0).WithMessage("DayNumber must be positive");

        RuleFor(x => x.Order)
            .GreaterThanOrEqualTo(0).WithMessage("Order must be non-negative");
    }
}
