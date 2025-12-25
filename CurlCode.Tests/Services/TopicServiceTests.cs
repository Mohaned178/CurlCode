using AutoMapper;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Topics;
using CurlCode.Application.Services.Topics;
using CurlCode.Domain.Entities.Problems;
using FluentAssertions;
using Moq;
using Xunit;

namespace CurlCode.Tests.Services;

public class TopicServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TopicService _topicService;

    public TopicServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _topicService = new TopicService(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldThrowValidationException_WhenNameExists()
    {
        // Arrange
        var dto = new CreateTopicDto { Name = "Arrays", Description = "Test" };
        _mockUnitOfWork.Setup(x => x.Topics.GetByNameAsync(dto.Name))
            .ReturnsAsync(new Topic { Id = 1, Name = "Arrays" });

        // Act
        Func<Task> act = async () => await _topicService.CreateTopicAsync(dto);

        // Assert
        await act.Should().ThrowAsync<CurlCode.Application.Common.Exceptions.ValidationException>()
            .WithMessage("Topic with this name already exists.");
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldSucceed_WhenNameIsUnique()
    {
        // Arrange
        var dto = new CreateTopicDto { Name = "NewTopic", Description = "Test" };
        _mockUnitOfWork.Setup(x => x.Topics.GetByNameAsync(dto.Name))
            .ReturnsAsync((Topic?)null);
        
        var topic = new Topic { Id = 1, Name = "NewTopic" };
        _mockMapper.Setup(x => x.Map<Topic>(dto)).Returns(topic);
        _mockMapper.Setup(x => x.Map<TopicDto>(topic)).Returns(new TopicDto { Id = 1, Name = "NewTopic" });

        // Act
        var result = await _topicService.CreateTopicAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("NewTopic");
        _mockUnitOfWork.Verify(x => x.Topics.AddAsync(It.IsAny<Topic>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
