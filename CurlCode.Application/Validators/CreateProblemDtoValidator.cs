using CurlCode.Application.DTOs.Problems;
using FluentValidation;

namespace CurlCode.Application.Validators;

public class CreateProblemDtoValidator : AbstractValidator<CreateProblemDto>
{
    public CreateProblemDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(3, 200).WithMessage("Title must be between 3 and 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(10).WithMessage("Description must be at least 10 characters");

        RuleFor(x => x.Constraints)
            .MaximumLength(2000).WithMessage("Constraints cannot exceed 2000 characters");

        RuleFor(x => x.Examples)
            .MaximumLength(2000).WithMessage("Examples cannot exceed 2000 characters");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level");

        RuleFor(x => x.TimeLimitMs)
            .InclusiveBetween(100, 10000).WithMessage("TimeLimitMs must be between 100 and 10000 milliseconds");

        RuleFor(x => x.MemoryLimitMb)
            .InclusiveBetween(64, 1024).WithMessage("MemoryLimitMb must be between 64 and 1024 MB");
    }
}
