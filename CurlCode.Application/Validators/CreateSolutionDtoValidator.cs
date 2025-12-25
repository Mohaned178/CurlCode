using CurlCode.Application.DTOs.Solutions;
using FluentValidation;

namespace CurlCode.Application.Validators;

public class CreateSolutionDtoValidator : AbstractValidator<CreateSolutionDto>
{
    public CreateSolutionDtoValidator()
    {
        RuleFor(x => x.ProblemId)
            .GreaterThan(0).WithMessage("Invalid Problem ID");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(3, 200).WithMessage("Title must be between 3 and 200 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MinimumLength(10).WithMessage("Content must be at least 10 characters");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required");
    }
}
