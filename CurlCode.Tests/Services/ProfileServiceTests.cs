using AutoMapper;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Profiles;
using CurlCode.Application.Services.Profiles;
using CurlCode.Domain.Entities.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace CurlCode.Tests.Services;

public class ProfileServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ProfileService _profileService;

    public ProfileServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _mockMapper = new Mock<IMapper>();
        
        _profileService = new ProfileService(_mockUnitOfWork.Object, _mockUserManager.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetProfileByUsernameAsync_ShouldReturnNull_WhenUserNotFound()
    {
        _mockUserManager.Setup(x => x.FindByNameAsync("unknown"))
            .ReturnsAsync((ApplicationUser?)null);

        var result = await _profileService.GetProfileByUsernameAsync("unknown");

        result.Should().BeNull();
    }
}
