using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.DTOs.Profiles;
using CurlCode.Application.Services.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CurlCode.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profileService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDistributedCache _cache;
    private const int CACHE_EXPIRATION_MINUTES = 5;

    public ProfilesController(
        IProfileService profileService,
        ICurrentUserService currentUserService,
        IDistributedCache cache)
    {
        _profileService = profileService;
        _currentUserService = currentUserService;
        _cache = cache;
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> GetMyProfile()
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var profile = await _profileService.GetMyProfileAsync(_currentUserService.UserId);
        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpPost("me")]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> CreateMyProfile([FromBody] UpdateProfileDto dto)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var profile = await _profileService.CreateOrUpdateProfileAsync(_currentUserService.UserId, dto);
        return Ok(profile);
    }

    [HttpPatch("me")]
    [Authorize]
    public async Task<ActionResult<ProfileDto>> UpdateMyProfile([FromBody] UpdateProfileDto dto)
    {
        if (!_currentUserService.IsAuthenticated || string.IsNullOrEmpty(_currentUserService.UserId))
        {
            return Unauthorized();
        }

        var profile = await _profileService.UpdateProfileAsync(_currentUserService.UserId, dto);
        
        // Invalidate profile cache for this user
        var user = await _profileService.GetMyProfileAsync(_currentUserService.UserId);
        if (user != null)
        {
            await _cache.RemoveAsync($"profile_username_{user.UserName.ToLowerInvariant()}");
        }
        
        return Ok(profile);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<ProfileDto>> GetProfileByUsername(string username)
    {
        var cacheKey = $"profile_username_{username.ToLowerInvariant()}";
        var cached = await _cache.GetStringAsync(cacheKey);
        ProfileDto? profile = null;
        if (!string.IsNullOrEmpty(cached))
            profile = JsonSerializer.Deserialize<ProfileDto>(cached);
        if (profile == null)
        {
            profile = await _profileService.GetProfileByUsernameAsync(username);
            if (profile == null) return NotFound();
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(profile), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            });
        }
        return Ok(profile);
    }
}






