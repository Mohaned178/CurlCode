using AutoMapper;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Auth;
using CurlCode.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CurlCode.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Specific domain restrictions removed to allow all users
        if (request.Email.Any(ch => char.IsUpper(ch)))
        {
            throw new Common.Exceptions.ValidationException("Email must be all lowercase.");
        }

        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new Common.Exceptions.ValidationException("Email is already registered.");
        }

        existingUser = await _userManager.FindByNameAsync(request.UserName);
        if (existingUser != null)
        {
            throw new Common.Exceptions.ValidationException("Username is already taken.");
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new Common.Exceptions.ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        // Create user profile - FirstName and LastName will be updated when user completes profile
        var profile = new UserProfile
        {
            UserId = user.Id,
            FirstName = string.Empty, // Will be updated when user completes profile
            LastName = string.Empty // Will be updated when user completes profile
        };
        await _unitOfWork.Profiles.AddAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            IsAdmin = user.IsAdmin
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Try finding by email first
        var user = await _userManager.FindByEmailAsync(request.EmailOrUserName);
        if (user == null)
        {
            // Fallback to username
            user = await _userManager.FindByNameAsync(request.EmailOrUserName);
        }
        if (user == null)
        {
            throw new Common.Exceptions.ValidationException("Invalid email/username or password.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            throw new Common.Exceptions.ValidationException("Invalid email/username or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            IsAdmin = user.IsAdmin
        };
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.EmailOrUserName)
                   ?? await _userManager.FindByNameAsync(request.EmailOrUserName);
        if (user == null)
            throw new Common.Exceptions.ValidationException("User not found.");

        var valid = await _userManager.VerifyUserTokenAsync(
            user,
            TokenOptions.DefaultProvider,
            "RefreshToken",
            request.RefreshToken);

        if (!valid)
            throw new Common.Exceptions.ValidationException("Invalid refresh token.");

        var token = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user);

        return new AuthResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            IsAdmin = user.IsAdmin
        };
    }

    public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Do not reveal user existence
            return new ForgotPasswordResponse { EmailSent = true };
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetLink = $"https://your-frontend-url/reset-password?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";

        var subject = "CurlCode - Reset your password";
        var body = $@"<p>Hello {user.UserName},</p>
                      <p>You requested a password reset. Click the link below to reset your password:</p>
                      <p><a href=""{resetLink}"">Reset Password</a></p>
                      <p>If you did not request this, you can safely ignore this email.</p>";

        await _emailSender.SendEmailAsync(request.Email, subject, body);

        return new ForgotPasswordResponse { EmailSent = true };
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
                   ?? throw new Common.Exceptions.NotFoundException(nameof(ApplicationUser), request.Email);

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Common.Exceptions.ValidationException(errors);
        }
    }

    public async Task LogoutAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;
        await _signInManager.SignOutAsync();
    }

    private async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
    {
        return await _userManager.GenerateUserTokenAsync(
            user,
            TokenOptions.DefaultProvider,
            "RefreshToken");
    }
}




