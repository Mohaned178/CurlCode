using CurlCode.Application.DTOs.Topics;
using CurlCode.Application.Services.Topics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly ITopicService _topicService;
    private readonly IDistributedCache _cache;
    private const int CACHE_EXPIRATION_MINUTES = 5;

    public TopicsController(ITopicService topicService, IDistributedCache cache)
    {
        _topicService = topicService;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TopicDto>>> GetAllTopics()
    {
        const string cacheKey = "topics_all";
        var cached = await _cache.GetStringAsync(cacheKey);
        IEnumerable<TopicDto>? topics = null;
        if (!string.IsNullOrEmpty(cached))
            topics = JsonSerializer.Deserialize<IEnumerable<TopicDto>>(cached);
        if (topics == null)
        {
            topics = await _topicService.GetAllTopicsAsync();
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(topics), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });
        }
        return Ok(topics);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TopicDto>> CreateTopic([FromBody] CreateTopicDto dto)
    {
        var topic = await _topicService.CreateTopicAsync(dto);
        
        // Invalidate topics cache
        await _cache.RemoveAsync("topics_all");
        
        return CreatedAtAction(nameof(GetAllTopics), new { id = topic.Id }, topic);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TopicDto>> UpdateTopic(int id, [FromBody] CreateTopicDto dto)
    {
        var topic = await _topicService.UpdateTopicAsync(id, dto);
        
        // Invalidate topics cache
        await _cache.RemoveAsync("topics_all");
        
        return Ok(topic);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        await _topicService.DeleteTopicAsync(id);
        
        // Invalidate topics cache
        await _cache.RemoveAsync("topics_all");
        
        return NoContent();
    }
}






