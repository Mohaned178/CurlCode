using CurlCode.Application.DTOs.Submissions;
using FluentValidation;

namespace CurlCode.Application.Validators;

public class SubmitCodeRequestValidator : AbstractValidator<SubmitCodeRequest>
{
    public SubmitCodeRequestValidator()
    {
        RuleFor(x => x.ProblemId)
            .GreaterThan(0).WithMessage("Invalid Problem ID");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required");

        RuleFor(x => x.Language)
            .IsInEnum().WithMessage("Invalid programming language");
    }
}
