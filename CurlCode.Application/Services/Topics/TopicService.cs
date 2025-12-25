using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Topics;
using CurlCode.Domain.Entities.Problems;

namespace CurlCode.Application.Services.Topics;

public class TopicService : ITopicService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TopicService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TopicDto>> GetAllTopicsAsync()
    {
        var topics = await _unitOfWork.Topics.GetAllAsync();
        return _mapper.Map<List<TopicDto>>(topics);
    }

    public async Task<TopicDto> CreateTopicAsync(CreateTopicDto dto)
    {
        var existingTopic = await _unitOfWork.Topics.GetByNameAsync(dto.Name);
        if (existingTopic != null)
        {
            throw new Common.Exceptions.ValidationException("Topic with this name already exists.");
        }

        var topic = _mapper.Map<Topic>(dto);
        await _unitOfWork.Topics.AddAsync(topic);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TopicDto>(topic);
    }

    public async Task<TopicDto> UpdateTopicAsync(int id, CreateTopicDto dto)
    {
        var topic = await _unitOfWork.Topics.GetByIdAsync(id);
        if (topic == null)
        {
            throw new NotFoundException(nameof(Topic), id);
        }

        var existingTopic = await _unitOfWork.Topics.GetByNameAsync(dto.Name);
        if (existingTopic != null && existingTopic.Id != id)
        {
            throw new Common.Exceptions.ValidationException("Topic with this name already exists.");
        }

        topic.Name = dto.Name;
        topic.Description = dto.Description;
        topic.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Topics.UpdateAsync(topic);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TopicDto>(topic);
    }

    public async Task DeleteTopicAsync(int id)
    {
        var topic = await _unitOfWork.Topics.GetByIdAsync(id);
        if (topic == null)
        {
            throw new NotFoundException(nameof(Topic), id);
        }

        await _unitOfWork.Topics.DeleteAsync(topic);
        await _unitOfWork.SaveChangesAsync();
    }
}






