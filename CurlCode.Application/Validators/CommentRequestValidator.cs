using CurlCode.Application.DTOs.Problems;
using FluentValidation;

namespace CurlCode.Application.Validators;

public class CommentRequestValidator : AbstractValidator<CommentRequest>
{
    public CommentRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Comment content is required")
            .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters");
    }
}
