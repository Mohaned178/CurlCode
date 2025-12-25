using AutoMapper;
using CurlCode.Application.DTOs.Problems;
using CurlCode.Application.DTOs.Profiles;
using CurlCode.Application.DTOs.Solutions;
using CurlCode.Application.DTOs.StudyPlans;
using CurlCode.Application.DTOs.Submissions;
using CurlCode.Application.DTOs.Topics;
using CurlCode.Domain.Entities.Community;
using CurlCode.Domain.Entities.Identity;
using CurlCode.Domain.Entities.Problems;
using CurlCode.Domain.Entities.StudyPlans;
using CurlCode.Domain.Entities.Submissions;

namespace CurlCode.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Problems
        CreateMap<Problem, ProblemDto>()
            .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.ProblemTopics.Select(pt => pt.Topic.Name).ToList()));
        
        CreateMap<Problem, ProblemDetailDto>()
            .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.ProblemTopics.Select(pt => pt.Topic.Name).ToList()));
        
        CreateMap<CreateProblemDto, Problem>();

        // Topics
        CreateMap<Topic, TopicDto>();
        CreateMap<CreateTopicDto, Topic>();

        // Submissions
        CreateMap<Submission, SubmissionResultDto>()
            .ForMember(dest => dest.ProblemTitle, opt => opt.MapFrom(src => src.Problem.Title));

        // Solutions
        CreateMap<Solution, SolutionDto>()
            .ForMember(dest => dest.ProblemTitle, opt => opt.MapFrom(src => src.Problem.Title))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.IsLikedByCurrentUser, opt => opt.Ignore());
        
        CreateMap<CreateSolutionDto, Solution>();

        // Profiles
        CreateMap<UserProfile, ProfileDto>();

        // StudyPlans
        CreateMap<StudyPlan, StudyPlanDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
        CreateMap<StudyPlanItem, StudyPlanItemDto>()
            .ForMember(dest => dest.ProblemTitle, opt => opt.MapFrom(src => src.Problem.Title))
            .ForMember(dest => dest.ProblemDifficulty, opt => opt.MapFrom(src => src.Problem.Difficulty));
        
        CreateMap<StudyPlanProgress, StudyPlanProgressDto>()
            .ForMember(dest => dest.StudyPlanTitle, opt => opt.MapFrom(src => src.StudyPlan.Title));
        
        CreateMap<StudyPlanItemProgress, StudyPlanItemProgressDto>()
            .ForMember(dest => dest.ProblemId, opt => opt.MapFrom(src => src.StudyPlanItem.ProblemId))
            .ForMember(dest => dest.ProblemTitle, opt => opt.MapFrom(src => src.StudyPlanItem.Problem.Title));
    }
}

