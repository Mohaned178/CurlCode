using AutoMapper;
using CurlCode.Application.Common.Exceptions;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.DTOs.Profiles;
using CurlCode.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace CurlCode.Application.Services.Profiles;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public ProfileService(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ProfileDto?> GetMyProfileAsync(string userId)
    {
        var profile = await _unitOfWork.Profiles.GetByUserIdAsync(userId);
        if (profile == null)
        {
            return null;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        // Return all information from UserProfile (FirstName, LastName) and ApplicationUser (Email, UserName)
        var profileDto = _mapper.Map<ProfileDto>(profile);
        profileDto.Email = user.Email!;
        profileDto.UserName = user.UserName!;
        profileDto.CreatedAt = user.CreatedAt;

        return profileDto;
    }

    public async Task<ProfileDto?> GetProfileByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return null;
        }

        var profile = await _unitOfWork.Profiles.GetByUserIdAsync(user.Id);
        if (profile == null)
        {
            return null;
        }

        // Return all information from UserProfile (which includes FirstName, LastName, Email, UserName)
        var profileDto = _mapper.Map<ProfileDto>(profile);
        profileDto.CreatedAt = user.CreatedAt;

        // Calculate rank
        profileDto.Rank = await _unitOfWork.Profiles.GetUserRankAsync(profile.TotalScore);

        return profileDto;
    }

    public async Task<ProfileDto> CreateOrUpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ValidationException("UserId cannot be empty");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        var profile = await _unitOfWork.Profiles.GetByUserIdAsync(userId);
        
        if (profile == null)
        {
            // Create new profile with FirstName and LastName
            profile = new UserProfile
            {
                UserId = userId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Bio = dto.Bio,
                ProfileImageUrl = dto.ProfileImageUrl,
                GitHubUrl = dto.GitHubUrl,
                LinkedInUrl = dto.LinkedInUrl,
                WebsiteUrl = dto.WebsiteUrl,
                Country = dto.Country,
                University = dto.University,
                Work = dto.Work,
                Major = dto.Major,
                DateOfBirth = dto.DateOfBirth
            };
            await _unitOfWork.Profiles.AddAsync(profile);
        }
        else
        {
            // Update existing profile - only FirstName, LastName and optional fields
            profile.FirstName = dto.FirstName;
            profile.LastName = dto.LastName;

            if (dto.Bio != null) profile.Bio = dto.Bio;
            if (dto.ProfileImageUrl != null) profile.ProfileImageUrl = dto.ProfileImageUrl;
            if (dto.GitHubUrl != null) profile.GitHubUrl = dto.GitHubUrl;
            if (dto.LinkedInUrl != null) profile.LinkedInUrl = dto.LinkedInUrl;
            if (dto.WebsiteUrl != null) profile.WebsiteUrl = dto.WebsiteUrl;
            if (dto.Country != null) profile.Country = dto.Country;
            if (dto.University != null) profile.University = dto.University;
            if (dto.Work != null) profile.Work = dto.Work;
            if (dto.Major != null) profile.Major = dto.Major;
            if (dto.DateOfBirth.HasValue) profile.DateOfBirth = dto.DateOfBirth;
            profile.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Profiles.UpdateAsync(profile);
        }
        
        await _unitOfWork.SaveChangesAsync();

        // Return profile with Email and UserName from ApplicationUser
        var profileDto = _mapper.Map<ProfileDto>(profile);
        profileDto.Email = user.Email!;
        profileDto.UserName = user.UserName!;
        profileDto.CreatedAt = user.CreatedAt;

        return profileDto;
    }

    public async Task<ProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ValidationException("UserId cannot be empty");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), userId);
        }

        var profile = await _unitOfWork.Profiles.GetByUserIdAsync(userId);
        if (profile == null)
        {
            throw new NotFoundException(nameof(UserProfile), userId);
        }

        if (!string.IsNullOrWhiteSpace(dto.FirstName))
            profile.FirstName = dto.FirstName;

        if (!string.IsNullOrWhiteSpace(dto.LastName))
            profile.LastName = dto.LastName;


        if (dto.Bio != null) profile.Bio = dto.Bio;
        if (dto.ProfileImageUrl != null) profile.ProfileImageUrl = dto.ProfileImageUrl;
        if (dto.GitHubUrl != null) profile.GitHubUrl = dto.GitHubUrl;
        if (dto.LinkedInUrl != null) profile.LinkedInUrl = dto.LinkedInUrl;
        if (dto.WebsiteUrl != null) profile.WebsiteUrl = dto.WebsiteUrl;
        if (dto.Country != null) profile.Country = dto.Country;
        if (dto.University != null) profile.University = dto.University;
        if (dto.Work != null) profile.Work = dto.Work;
        if (dto.Major != null) profile.Major = dto.Major;
        if (dto.DateOfBirth.HasValue) profile.DateOfBirth = dto.DateOfBirth;
        profile.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Profiles.UpdateAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        var profileDto = _mapper.Map<ProfileDto>(profile);
        profileDto.Email = user.Email!;
        profileDto.UserName = user.UserName!;
        profileDto.CreatedAt = user.CreatedAt;

        return profileDto;
    }
}





