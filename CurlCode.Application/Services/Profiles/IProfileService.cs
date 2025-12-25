using CurlCode.Application.DTOs.Profiles;

namespace CurlCode.Application.Services.Profiles;

public interface IProfileService
{
    Task<ProfileDto?> GetMyProfileAsync(string userId);
    Task<ProfileDto?> GetProfileByUsernameAsync(string username);
    Task<ProfileDto> CreateOrUpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);
}






