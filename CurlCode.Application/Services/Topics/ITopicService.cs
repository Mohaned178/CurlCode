using CurlCode.Application.DTOs.Topics;

namespace CurlCode.Application.Services.Topics;

public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetAllTopicsAsync();
    Task<TopicDto> CreateTopicAsync(CreateTopicDto dto);
    Task<TopicDto> UpdateTopicAsync(int id, CreateTopicDto dto);
    Task DeleteTopicAsync(int id);
}






