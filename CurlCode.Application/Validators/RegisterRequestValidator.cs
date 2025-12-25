using CurlCode.Application.DTOs.Auth;
using FluentValidation;

namespace CurlCode.Application.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters")
            .Must(email => 
            {
                if (string.IsNullOrEmpty(email)) return false;
                var allowedDomains = new[] { "gmail.com", "outlook.com", "hotmail.com", "yahoo.com", "icloud.com" };
                var domain = email.Split('@').LastOrDefault()?.ToLower();
                return domain != null && allowedDomains.Contains(domain);
            })
            .WithMessage("Email must be from a valid provider (Gmail, Outlook, Hotmail, Yahoo, iCloud).");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required")
            .MinimumLength(3).WithMessage("UserName must be at least 3 characters")
            .MaximumLength(256).WithMessage("UserName cannot exceed 256 characters")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("UserName can only contain letters, numbers, and underscores");
    }
}
