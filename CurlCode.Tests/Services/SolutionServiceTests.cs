using AutoMapper;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Solutions;
using CurlCode.Application.Services.Solutions;
using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Problems;
using FluentAssertions;
using Moq;
using Xunit;

namespace CurlCode.Tests.Services;

public class SolutionServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SolutionService _solutionService;

    public SolutionServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _solutionService = new SolutionService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateSolutionAsync_ShouldThrowNotFound_WhenProblemDoesNotExist()
    {
        // Arrange
        var dto = new CreateSolutionDto { ProblemId = 999, Title = "Test", Content = "Test", Code = "code" };
        _mockUnitOfWork.Setup(x => x.Problems.GetByIdAsync(dto.ProblemId))
            .ReturnsAsync((Problem?)null);

        // Act
        Func<Task> act = async () => await _solutionService.CreateSolutionAsync(dto, "user1");

        // Assert
        await act.Should().ThrowAsync<CurlCode.Application.Common.Exceptions.NotFoundException>();
    }

    [Fact]
    public async Task UpdateSolutionAsync_ShouldThrowValidationException_WhenUserIsNotOwner()
    {
        // Arrange
        var solutionId = 1;
        var dto = new CreateSolutionDto { Title = "Update" };
        var solution = new Solution { Id = solutionId, UserId = "user1" };

        _mockUnitOfWork.Setup(x => x.Solutions.GetByIdAsync(solutionId))
            .ReturnsAsync(solution);

        // Act
        Func<Task> act = async () => await _solutionService.UpdateSolutionAsync(solutionId, dto, "user2");

        // Assert
        // Assuming your service throws ValidationException for ownership check
        await act.Should().ThrowAsync<CurlCode.Application.Common.Exceptions.ValidationException>()
            .WithMessage("You can only update your own solutions.");
    }
}
