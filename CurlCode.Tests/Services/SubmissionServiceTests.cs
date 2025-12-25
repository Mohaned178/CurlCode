using AutoMapper;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Submissions;
using CurlCode.Application.Services.Submissions;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Entities.Submissions;
using CurlCode.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace CurlCode.Tests.Services;

public class SubmissionServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SubmissionService _submissionService;

    public SubmissionServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        
        // Mock Repositories
        _mockUnitOfWork.Setup(x => x.Problems).Returns(new Mock<IProblemRepository>().Object);
        _mockUnitOfWork.Setup(x => x.Submissions).Returns(new Mock<ISubmissionRepository>().Object);
        _mockUnitOfWork.Setup(x => x.Submissions.AddAsync(It.IsAny<Submission>()))
            .ReturnsAsync((Submission s) => s);
        _mockUnitOfWork.Setup(x => x.Submissions.UpdateAsync(It.IsAny<Submission>()))
            .Returns(Task.CompletedTask);

        _submissionService = new SubmissionService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task SubmitCodeAsync_ShouldProcessSubmission_WhenProblemExists()
    {
        // Arrange
        var request = new SubmitCodeRequest { ProblemId = 1, Code = "print('hello')", Language = ProgrammingLanguage.Python };
        var problem = new Problem { Id = 1, Title = "Pro", TestCases = new List<TestCase>() };
        
        _mockUnitOfWork.Setup(x => x.Problems.GetByIdWithDetailsAsync(request.ProblemId))
            .ReturnsAsync(problem);
            
        var submission = new Submission { Id = 1, ProblemId = 1, Status = SubmissionStatus.Pending };
        _mockMapper.Setup(x => x.Map<SubmissionResultDto>(It.IsAny<Submission>()))
            .Returns(new SubmissionResultDto { Id = 1, Status = SubmissionStatus.Accepted });

        // Act
        var result = await _submissionService.SubmitCodeAsync(request, "user1");

        // Assert
        result.Should().NotBeNull();
        _mockUnitOfWork.Verify(x => x.Submissions.AddAsync(It.IsAny<Submission>()), Times.Once);
        // The service logic simulates processing and updates status
        _mockUnitOfWork.Verify(x => x.Submissions.UpdateAsync(It.IsAny<Submission>()), Times.Once);
    }
}
