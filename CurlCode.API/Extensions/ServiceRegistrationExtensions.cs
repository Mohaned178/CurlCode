using CurlCode.Application.Common.Mappings;
using CurlCode.Application.Contracts.Infrastructure;
using CurlCode.Application.Contracts.Persistence;
using CurlCode.Application.Services.Auth;
using CurlCode.Application.Services.Contests;
using CurlCode.Application.Services.Problems;
using CurlCode.Application.Services.Profiles;
using CurlCode.Application.Services.Solutions;
using CurlCode.Application.Services.StudyPlans;
using CurlCode.Application.Services.Submissions;
using CurlCode.Application.Services.Topics;
using CurlCode.Application.Validators;
using CurlCode.Infrastructure.Identity;
using CurlCode.Infrastructure.Persistence.Contexts;
using CurlCode.Infrastructure.Persistence.Repositories;
using CurlCode.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CurlCode.API.Extensions;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProblemService, ProblemService>();
        services.AddScoped<ITopicService, TopicService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<ISolutionService, SolutionService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IStudyPlanService, StudyPlanService>();
        services.AddScoped<IContestService, ContestService>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentityServices(configuration);

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IEmailSender, EmailSender>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "CurlCode:";
        });
        
        services.AddSingleton<IConnectionMultiplexer>(sp => 
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
        
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
