using AutoMapper;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.StudyPlans;
using CurlCode.Application.Services.StudyPlans;
using CurlCode.Domain.Entities.StudyPlans;
using CurlCode.Domain.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace CurlCode.Tests.Services;

public class StudyPlanServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly StudyPlanService _studyPlanService;

    public StudyPlanServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _studyPlanService = new StudyPlanService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateCustomStudyPlanAsync_ShouldCreatePlan_WhenItemsAreValid()
    {
        // Arrange
        var dto = new CreateStudyPlanDto 
        { 
            Title = "My Plan", 
            DurationDays = 10,
            Items = new List<StudyPlanItemCreateDto> 
            { 
                new StudyPlanItemCreateDto { ProblemId = 1, DayNumber = 1 } 
            }
        };
        
        _mockUnitOfWork.Setup(x => x.Problems.GetByIdAsync(1))
            .ReturnsAsync(new CurlCode.Domain.Entities.Problems.Problem { Id = 1 });
            
        var createdPlan = new StudyPlan { Id = 1, Title = "My Plan" };
        _mockUnitOfWork.Setup(x => x.StudyPlans.CreateCustomStudyPlanAsync(It.IsAny<StudyPlan>(), It.IsAny<List<StudyPlanItem>>()))
            .ReturnsAsync(createdPlan);
            
        _mockMapper.Setup(x => x.Map<StudyPlanDto>(createdPlan))
            .Returns(new StudyPlanDto { Id = 1, Title = "My Plan" });

        // Act
        var result = await _studyPlanService.CreateCustomStudyPlanAsync(dto, "user1");

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("My Plan");
    }
}
