using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Auth;
using CurlCode.Application.Services.Auth;
using CurlCode.Domain.Entities.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using AutoMapper;
using Xunit;

namespace CurlCode.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager; 
    private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        
        
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockMapper = new Mock<IMapper>();
        var mockEmailSender = new Mock<IEmailSender>();

        _authService = new AuthService(
            _mockUserManager.Object,
            null!, 
            _mockJwtTokenGenerator.Object,
            _mockUnitOfWork.Object,
            mockMapper.Object,
            mockEmailSender.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowValidationException_WhenEmailExists()
    {
        var request = new RegisterRequest { Email = "test@gmail.com", Password = "Password123!", UserName = "testuser" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(new ApplicationUser());

        Func<Task> act = async () => await _authService.RegisterAsync(request);

        await act.Should().ThrowAsync<CurlCode.Application.Common.Exceptions.ValidationException>()
            .WithMessage("Email is already registered.");
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrowValidationException_WhenUserNameExists()
    {
        var request = new RegisterRequest { Email = "test@gmail.com", Password = "Password123!", UserName = "testuser" };
        _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser?)null);
        _mockUserManager.Setup(x => x.FindByNameAsync(request.UserName))
            .ReturnsAsync(new ApplicationUser());

        Func<Task> act = async () => await _authService.RegisterAsync(request);

        await act.Should().ThrowAsync<CurlCode.Application.Common.Exceptions.ValidationException>()
            .WithMessage("Username is already taken.");
    }
}
