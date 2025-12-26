using CurlCode.Application.Common.Constants;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Problems;
using CurlCode.Application.Services.Problems;
using CurlCode.Domain.Entities.Problems;
using FluentAssertions;
using Moq;
using AutoMapper;
using Xunit;

namespace CurlCode.Tests.Services;

public class ProblemServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ICacheService> _mockCacheService;
    private readonly ProblemService _problemService;

    public ProblemServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockCacheService = new Mock<ICacheService>();
        _problemService = new ProblemService(_mockUnitOfWork.Object, _mockMapper.Object, _mockCacheService.Object);
    }

    [Fact]
    public async Task GetProblemByIdAsync_ShouldReturnFromCache_WhenCacheExists()
    {
        var problemId = 1;
        var cachedDto = new ProblemDetailDto { Id = problemId, Title = "Cached Problem" };
        var cacheKey = CacheKeys.GetProblemDetailKey(problemId);

        _mockCacheService.Setup(x => x.GetAsync<ProblemDetailDto>(cacheKey))
            .ReturnsAsync(cachedDto);

        var result = await _problemService.GetProblemByIdAsync(problemId);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(cachedDto);
        _mockUnitOfWork.Verify(x => x.Problems.GetByIdWithDetailsAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetProblemByIdAsync_ShouldResultFromDbAndSetCache_WhenCacheDoesNotExist()
    {
        var problemId = 1;
        var problemEntity = new Problem { Id = problemId, Title = "Db Problem" };
        var problemDto = new ProblemDetailDto { Id = problemId, Title = "Db Problem" };
        var cacheKey = CacheKeys.GetProblemDetailKey(problemId);

        _mockCacheService.Setup(x => x.GetAsync<ProblemDetailDto>(cacheKey))
            .ReturnsAsync((ProblemDetailDto?)null);

        _mockUnitOfWork.Setup(x => x.Problems.GetByIdWithDetailsAsync(problemId))
            .ReturnsAsync(problemEntity);

        _mockMapper.Setup(x => x.Map<ProblemDetailDto>(problemEntity))
            .Returns(problemDto);

        var result = await _problemService.GetProblemByIdAsync(problemId);

        result.Should().NotBeNull();
        result.Title.Should().Be("Db Problem");
        _mockUnitOfWork.Verify(x => x.Problems.GetByIdWithDetailsAsync(problemId), Times.Once);
        _mockCacheService.Verify(x => x.SetAsync(cacheKey, problemDto, It.IsAny<TimeSpan?>()), Times.Once);
    }
}
